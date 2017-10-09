using System;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace cShART
{
    public class ARTClient
    {
        protected ArrayList sixDobjects = new ArrayList();
        protected ArrayList flysticks = new ArrayList();
        protected UdpClient dsocket;
        protected ArrayList flystickListener = new ArrayList();
        protected int frame;
        protected string IP;
        protected Hashtable ListenerToStickTable = new Hashtable();
        protected int numberOf6dTargets;
        protected int numberOfFlysticks;
        protected ArrayList observers = new ArrayList();
        protected int port;
        protected string rawData = "";
        protected bool running;
        protected Hashtable StickToListenerTable = new Hashtable();
        protected Thread Thr;
        protected float timestamp;

        public ARTClient(int port, string ip = "127.0.0.1")
        {
            IP = ip;
            this.port = port;
        }

        public void Pause()
        {
            running = false;
        }
        
        /// <summary>
        /// Check if an ART-object exists in the client instance.
        /// </summary>
        /// <param name="tracker">Decide wether you want to check for a 6d-object or a flystick-object.</param>
        /// <param name="id">Insert the ID you are looking for.</param>
        /// <returns>Returns true if an object with this ID exists.</returns>
        public bool ObjectExistsByID(ARTTracker tracker, int id)
        {
            if (tracker == ARTTracker.SixD)
            {
                foreach (ART6d art6Dobject in sixDobjects)
                {
                    if (art6Dobject.GetId() == id)
                    {
                        return true;
                    }
                }
            }
            if (tracker == ARTTracker.Flystick)
            {
                foreach (ARTFlystick flystick in flysticks)
                {
                    if (flystick.GetId() == id)
                    {
                        return true;
                    }
                }
            }
            return false;

        }

        public void AddFlystickListener(IFlystickListener flistener, ARTFlystick uniqueFlystick)
        {
            if (!ListenerToStickTable.ContainsValue(uniqueFlystick))
            {
                flystickListener.Add(flistener);
                ListenerToStickTable.Add(flistener, uniqueFlystick);
                StickToListenerTable.Add(uniqueFlystick, flistener);
            }
        }

        public void RemoveFlystickListener(IFlystickListener flistener)
        {
            if (flystickListener.Contains(flistener))
            {
                flystickListener.Remove(flistener);
                var stick = (ARTFlystick) ListenerToStickTable[flistener];
                ListenerToStickTable.Remove(flistener);
                StickToListenerTable.Remove(stick);
            }
        }

        protected void UpdateFlystickListener(IFlystickListener flistener)
        {
            ARTFlystick flystick = null;
            float[] xyFloats = null;
            BitArray binBStates = null;
            if (ListenerToStickTable.ContainsKey(flistener))
            {
                flystick = (ARTFlystick) ListenerToStickTable[flistener];
                xyFloats = flystick.GetStickXYPos();
                binBStates = new BitArray(new[] {flystick.GetButtonStates()});
            }

            if (flystick != null)
            {
                if (binBStates[3]) //leftbutton
                    flistener.OnLeftButtonPress();
                if (binBStates[2]) //middlebutton
                    flistener.OnMiddleButtonPress();
                if (binBStates[1]) //rightbutton
                    flistener.OnRightButtonPress();
                if (binBStates[0]) //trigger
                    flistener.OnTriggerPress();
                if (xyFloats[0] > 0.1F)
                    flistener.OnStickRight();
                if (xyFloats[0] < -0.1F)
                    flistener.OnStickLeft();
                if (xyFloats[1] > 0.1F)
                    flistener.OnStickUp();
                if (xyFloats[1] < 0.1F)
                    flistener.OnStickDown();
            }
        }

        public ARTFlystick GetFlystickByListener(IFlystickListener listener)
        {
            return ListenerToStickTable[listener] as ARTFlystick;
        }

        /// <summary>
        /// In the very unlikely case that you have too many flysticks than you could identify
        /// you can use this method to get a flystick that has not yet been assigned to a flystick listener.
        /// </summary>
        /// <returns>Returns a flystick that has not been assigned to a flystick listener.</returns>
        public ARTFlystick GetUnusedFlystick()
        {
            if (flysticks.Count != 0 && flystickListener.Count != 0)
                for (var i = 0; i < flystickListener.Count; i++)
                    if (!ListenerToStickTable.ContainsKey(flystickListener[i]))
                        return flystickListener[i] as ARTFlystick;
            return null;
        }

        public void AddARTObserver(ARTObserver observer)
        {
            observers.Add(observer);
        }

        public void RemoveARTObserver(ARTObserver observer)
        {
            observers.Remove(observer);
        }

        public string GetRawData()
        {
            return rawData;
        }

        public int GetFrame()
        {
            return frame;
        }

        public float GetTimestamp()
        {
            return timestamp;
        }

        public int GetNumberOf6DTargets()
        {
            return numberOf6dTargets;
        }

        public int GetNumberOfFlysticks()
        {
            return numberOfFlysticks;
        }

        public int GetNumberOfTargets()
        {
            return GetNumberOf6DTargets() + GetNumberOfFlysticks();
        }

        public ArrayList Get6DObjects()
        {
            return sixDobjects;
        }

        public ART6d Get6DObjectById(int id)
        {
            for (var i = 0; i < sixDobjects.Count; i++)
                if (((ART6d) sixDobjects[i]).GetId() == id)
                    return (ART6d) sixDobjects[i];
            return ART6d.Empty();
        }

        public ArrayList GetFlysticks()
        {
            return flysticks;
        }

        public ARTFlystick GetFlystickById(int id)
        {
            for (var i = 0; i < flysticks.Count; i++)
                if (((ARTFlystick) flysticks[i]).GetId() == id)
                    return (ARTFlystick) flysticks[i];
            return ARTFlystick.Empty();
        }

        protected int ParseInt(string msg)
        {
            msg = msg.Replace(" ", "");
            var res = 0;
            try
            {
                res = int.Parse(msg);
            }
            catch (Exception)
            {
            }
            return res;
        }

        protected float ParseFloat(string msg)
        {
            msg = msg.Replace(" ", "");
            var res = 0.0F;
            try
            {
                res = float.Parse(msg);
            }
            catch (Exception)
            {
            }
            return res;
        }

        protected float[] ParseFloatArray(string[] msg)
        {
            var res = new float[msg.Length];
            for (var i = 0; i < msg.Length; i++)
                res[i] = ParseFloat(msg[i]);
            return res;
        }

        //protected void add6dObject(ART6d obj)
        //{
        //    for (int i = 0; i < art6dobjects.Count; i++)
        //    {
        //        if (((ART6d)art6dobjects[i]).getId() == obj.getId())
        //        {
        //            art6dobjects.RemoveAt(i);
        //        }
        //    }
        //    art6dobjects.Add(obj);
        //}

        protected void Add6DObject(ART6d obj)
        {
            var exists = false;
            for (var i = 0; i < sixDobjects.Count; i++)
                if (((ART6d) sixDobjects[i]).GetId() == obj.GetId())
                {
                    sixDobjects.RemoveAt(i);
                    sixDobjects.Insert(i, obj);
                    exists = true;
                }
            if (!exists)
                sixDobjects.Add(obj);
        }

        //protected void addFlystick(ARTFlystick obj)
        //{
        //    for (int i = 0; i < artFlysticks.Count; i++)
        //    {
        //        if (((ARTFlystick)artFlysticks[i]).getId() == obj.getId())
        //        {
        //            artFlysticks.RemoveAt(i);
        //        }
        //    }
        //    artFlysticks.Add(obj);
        //}

        protected void AddFlystick(ARTFlystick obj)
        {
            var exists = false;
            for (var i = 0; i < flysticks.Count; i++)
                if (((ARTFlystick) flysticks[i]).GetId() == obj.GetId())
                {
                    flysticks.RemoveAt(i);
                    flysticks.Insert(i, obj);
                    exists = true;
                }
            if (!exists)
                flysticks.Add(obj);
        }

        protected void Parse6D(string msg)
        {
            var id = 0;
            var position = ARTPoint.Empty();
            var angle = ARTAngle.Empty();
            var matrix = ARTMatrix.Empty();


            msg = msg.Replace("]", "");
            msg = msg.Replace("[", "x");
            var tmp = msg.Split('x');
            if (tmp.Length >= 4)
            {
                numberOf6dTargets = ParseInt(tmp[0]);
                var tmp2 = tmp[1].Split(' ');
                if (tmp2.Length >= 2)
                    id = ParseInt(tmp2[0]);
                tmp2 = tmp[2].Split(' ');
                if (tmp2.Length >= 6)
                {
                    position = new ARTPoint(ParseFloat(tmp2[0]),
                        ParseFloat(tmp2[1]), ParseFloat(tmp2[2]));
                    angle = new ARTAngle(ParseFloat(tmp2[3]), ParseFloat(tmp2[4]),
                        ParseFloat(tmp2[5]));
                }
                tmp2 = tmp[3].Split(' ');
                if (tmp2.Length >= 9)
                    matrix = new ARTMatrix(ParseFloatArray(tmp2));
                Add6DObject(new ART6d(id, position, angle, matrix));
                for (var i = 0; i < observers.Count; i++)
                    ((ARTObserver) observers[i]).On6DUpdate(this);
            }
        }

        protected void Parse6Df2(string msg)
        {
            var id = 1;
            var numberOfButtons = 0;
            var numberOfControllers = 0;
            var visible = false;
            var position = ARTPoint.Empty();
            var matrix = ARTMatrix.Empty();
            var buttonStates = 0;
            var controllerStates = new float[0];


            msg = msg.Replace("]", "");
            msg = msg.Replace("[", "x");
            var tmp = msg.Split('x');
            if (tmp.Length >= 5)
            {
                numberOfFlysticks = ParseInt(tmp[0]);
                var tmp2 = tmp[1].Split(' ');
                if (tmp2.Length >= 4)
                {
                    id = ParseInt(tmp2[0]);
                    if (ParseFloat(tmp2[1]) > 0.0F)
                        visible = true;
                    numberOfButtons = ParseInt(tmp2[2]);
                    numberOfControllers = ParseInt(tmp2[3]);
                }
                tmp2 = tmp[2].Split(' ');
                if (tmp2.Length >= 3)
                    position = new ARTPoint(ParseFloat(tmp2[0]),
                        ParseFloat(tmp2[1]), ParseFloat(tmp2[2]));
                tmp2 = tmp[3].Split(' ');
                if (tmp2.Length >= 9)
                    matrix = new ARTMatrix(ParseFloatArray(tmp2));
                tmp2 = tmp[4].Split(' ');
                if (tmp2.Length >= 1)
                    if (numberOfButtons > 0)
                    {
                        buttonStates = ParseInt(tmp2[0]);
                        controllerStates = new float[tmp2.Length - 1];
                        for (var i = 0; i < controllerStates.Length; i++)
                            controllerStates[i] = ParseFloat(tmp2[i + 1]);
                    }
                    else
                    {
                        controllerStates = new float[tmp2.Length];
                        for (var i = 0; i < controllerStates.Length; i++)
                            controllerStates[i] = ParseFloat(tmp2[i]);
                    }
                AddFlystick(new ARTFlystick(id, visible, numberOfButtons,
                    buttonStates, numberOfControllers, controllerStates,
                    position, matrix));
                for (var i = 0; i < observers.Count; i++)
                    ((ARTObserver) observers[i]).OnFlystickUpdate(this);
                foreach (IFlystickListener listener in flystickListener)
                    UpdateFlystickListener(listener);
            }
        }

        protected void DecodeMsg(string msg)
        {
            msg = msg.Replace("\n", "x").Replace("\r", "");
            var tmp = msg.Split('x');
            for (var i = 0; i < tmp.Length; i++)
                if (tmp[i].Contains("fr "))
                    frame = ParseInt(tmp[i].Replace("fr ", ""));
                else if (tmp[i].Contains("ts "))
                    timestamp = ParseFloat(tmp[i].Replace("ts ", ""));
                else if (tmp[i].Contains("6d "))
                    Parse6D(tmp[i].Replace("6d ", ""));
                else if (tmp[i].Contains("6df2 "))
                    Parse6Df2(tmp[i].Replace("6df2 ", ""));
        }

        public void Run()
        {
            var buffer = new byte[2048];
            var adr = new IPEndPoint(IPAddress.Parse(IP), port);
            running = true;
            var timeout = 0;
            try
            {
                dsocket = new UdpClient(port);
                Thr = new Thread(() =>
                {
                    while (running)
                    {
                        buffer = dsocket.Receive(ref adr);
                        rawData = Encoding.ASCII.GetString(buffer);
                        DecodeMsg(rawData);
                        for (var i = 0; i < observers.Count; i++)
                            ((ARTObserver) observers[i]).OnFrameUpdate(this);
                        if (rawData == "")
                            timeout++;
                        if (timeout == 100)
                        {
                            Thr.Abort(); //
                            running = false; //THIS HAS BEEN CHANGED orig: stop();
                            throw new Exception(
                                "Client timed out. Check if DTrack is running and sending packages over " + IP + ":" +
                                port + ".");
                        }
                    }
                });
                Thr.IsBackground = true;
                Thr.Start();
            }
            catch (Exception)
            {
            }
        }

        public void Stop()
        {
            if (Thr != null)
            {
                Thr.Abort();
                Thr = null;
            }
            if (dsocket != null)
            {
                dsocket.Close();
                dsocket = null;
            }
            running = false;
        }

        public string Status()
        {
            if (Thr != null && running)
            {
                if (dsocket != null)
                    return "Thread is up and running/Client alive and listening to " + port;
                return "Thread is up and running/Client seems to be dead";
            }
            return "Thread nor Client running";
        }
    }
}