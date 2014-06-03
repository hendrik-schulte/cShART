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
        protected int port;
        protected bool running;
        protected int frame = 0;
        protected float timestamp = 0.0F;
        protected int numberOf6dTargets = 0;
        protected int numberOfFlysticks = 0;
        protected ArrayList art6dobjects = new ArrayList();
        protected ArrayList artFlysticks = new ArrayList();
        protected ArrayList flystickListener = new ArrayList();
        protected String rawData = "";
        protected ArrayList observers = new ArrayList();
        protected UdpClient dsocket = null;
        protected Thread Thr;
        protected Hashtable ListenerToStickTable = new Hashtable();
        protected Hashtable StickToListenerTable = new Hashtable();
        protected string IP;
        public enum ARTTracker
        {
            sixd, fstick
        };

        public ARTClient(int port, string ip = "127.0.0.1")
        {
            IP = ip;
            this.port = port;
        }

        public void pause()
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
            if (tracker == ARTTracker.sixd)
            {
                foreach (ART6d art6Dobject in art6dobjects)
                {
                    if (art6Dobject.getId() == id)
                    {
                        return true;
                    }
                }
            }
            if (tracker == ARTTracker.fstick)
            {
                foreach (ARTFlystick flystick in artFlysticks)
                {
                    if (flystick.getId() == id)
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
                ARTFlystick stick = (ARTFlystick) ListenerToStickTable[flistener];
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
                binBStates = new BitArray(new int[] { flystick.getButtonStates() });
            }

            if (flystick != null)
            {
                if (binBStates[3]) //leftbutton
                {
                    flistener.onLeftButtonPress(flystick);
                }
                if (binBStates[2]) //middlebutton
                {
                    flistener.onMiddleButtonPress(flystick);
                }
                if (binBStates[1]) //rightbutton
                {
                    flistener.onRightButtonPress(flystick);
                }
                if (binBStates[0]) //trigger
                {
                    flistener.onTriggerPress(flystick);
                }
                if (xyFloats[0] > 0.1F)
                {
                    flistener.onStickRight(flystick);
                }
                if (xyFloats[0] < -0.1F)
                {
                    flistener.onStickLeft(flystick);
                }
                if (xyFloats[1] > 0.1F)
                {
                    flistener.onStickUp(flystick);
                }
                if (xyFloats[1] < 0.1F)
                {
                    flistener.onStickDown(flystick);
                } 
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
            if (artFlysticks.Count != 0 && flystickListener.Count != 0)
            {
                for (int i = 0; i < flystickListener.Count; i++)
                {
                    if (!ListenerToStickTable.ContainsKey(flystickListener[i]))
                    {
                        return flystickListener[i] as ARTFlystick;
                    }
                }
            }
            return null;
        }

        public void addARTObserver(ARTObserver observer)
        {
            observers.Add(observer);
        }

        public void removeARTObserver(ARTObserver observer)
        {
            observers.Remove(observer);
        }

        public String getRawData()
        {
            return rawData;
        }

        public int getFrame()
        {
            return frame;
        }

        public float getTimestamp()
        {
            return timestamp;
        }

        public int getNumberOf6dTargets()
        {
            return numberOf6dTargets;
        }

        public int getNumberOfFlysticks()
        {
            return numberOfFlysticks;
        }

        public int getNumberOfTargets()
        {
            return getNumberOf6dTargets() + getNumberOfFlysticks();
        }

        public ArrayList getART6dObjects()
        {
            return art6dobjects;
        }

        public ART6d getART6dObjectById(int id)
        {
            for (int i = 0; i < art6dobjects.Count; i++)
            {
                if (((ART6d) art6dobjects[i]).getId() == id)
                {
                    return (ART6d) art6dobjects[i];
                }
            }
            return ART6d.Empty();
        }

        public ArrayList getFlysticks()
        {
            return artFlysticks;
        }

        public ARTFlystick getARTFlystickById(int id)
        {
            for (int i = 0; i < artFlysticks.Count; i++)
            {
                if (((ARTFlystick) artFlysticks[i]).getId() == id)
                {
                    return (ARTFlystick) artFlysticks[i];
                }
            }
            return ARTFlystick.Empty();
        }

        protected int parseInt(String msg)
        {
            msg = msg.Replace(" ", "");
            int res = 0;
            try
            {
                res = int.Parse(msg);
            }
            catch (Exception localException)
            {
            }
            return res;
        }

        protected float parseFloat(String msg)
        {
            msg = msg.Replace(" ", "");
            float res = 0.0F;
            try
            {
                res = float.Parse(msg);
            }
            catch (Exception localException)
            {
            }
            return res;
        }

        protected float[] parseFloatArray(String[] msg)
        {
            var res = new float[msg.Length];
            for (int i = 0; i < msg.Length; i++)
            {
                res[i] = parseFloat(msg[i]);
            }
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

        protected void add6dObject(ART6d obj)
        {
            bool exists = false;
            for (int i = 0; i < art6dobjects.Count; i++)
            {
                if (((ART6d)art6dobjects[i]).getId() == obj.getId())
                {
                    art6dobjects.RemoveAt(i);
                    art6dobjects.Insert(i, obj);
                    exists = true;
                }
            }
            if (!exists)
            {
                art6dobjects.Add(obj);
            }
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

        protected void addFlystick(ARTFlystick obj)
        {
            bool exists = false;
            for (int i = 0; i < artFlysticks.Count; i++)
            {
                if (((ARTFlystick)artFlysticks[i]).getId() == obj.getId())
                {
                    artFlysticks.RemoveAt(i);
                    artFlysticks.Insert(i, obj);
                    exists = true;
                }
            }
            if (!exists)
            {
                artFlysticks.Add(obj);
            }
        }

        protected void parse6d(String msg)
        {
            int id = 0;
            ARTPoint position = ARTPoint.Empty();
            ARTAngle angle = ARTAngle.Empty();
            ARTMatrix matrix = ARTMatrix.Empty();


            msg = msg.Replace("]", "");
            msg = msg.Replace("[", "x");
            String[] tmp = msg.Split('x');
            if (tmp.Length >= 4)
            {
                numberOf6dTargets = parseInt(tmp[0]);
                String[] tmp2 = tmp[1].Split(' ');
                if (tmp2.Length >= 2)
                {
                    id = parseInt(tmp2[0]);
                }
                tmp2 = tmp[2].Split(' ');
                if (tmp2.Length >= 6)
                {
                    position = new ARTPoint(parseFloat(tmp2[0]),
                        parseFloat(tmp2[1]), parseFloat(tmp2[2]));
                    angle = new ARTAngle(parseFloat(tmp2[3]), parseFloat(tmp2[4]),
                        parseFloat(tmp2[5]));
                }
                tmp2 = tmp[3].Split(' ');
                if (tmp2.Length >= 9)
                {
                    matrix = new ARTMatrix(parseFloatArray(tmp2));
                }
                add6dObject(new ART6d(id, position, angle, matrix));
                for (int i = 0; i < observers.Count; i++)
                {
                    ((ARTObserver) observers[i]).on6dUpdate(this);
                }
            }
        }

        protected void parse6df2(String msg)
        {
            int id = 1;
            int numberOfButtons = 0;
            int numberOfControllers = 0;
            bool visible = false;
            ARTPoint position = ARTPoint.Empty();
            ARTMatrix matrix = ARTMatrix.Empty();
            int buttonStates = 0;
            var controllerStates = new float[0];


            msg = msg.Replace("]", "");
            msg = msg.Replace("[", "x");
            String[] tmp = msg.Split('x');
            if (tmp.Length >= 5)
            {
                numberOfFlysticks = parseInt(tmp[0]);
                String[] tmp2 = tmp[1].Split(' ');
                if (tmp2.Length >= 4)
                {
                    id = parseInt(tmp2[0]);
                    if (parseFloat(tmp2[1]) > 0.0F)
                    {
                        visible = true;
                    }
                    numberOfButtons = parseInt(tmp2[2]);
                    numberOfControllers = parseInt(tmp2[3]);
                }
                tmp2 = tmp[2].Split(' ');
                if (tmp2.Length >= 3)
                {
                    position = new ARTPoint(parseFloat(tmp2[0]),
                        parseFloat(tmp2[1]), parseFloat(tmp2[2]));
                }
                tmp2 = tmp[3].Split(' ');
                if (tmp2.Length >= 9)
                {
                    matrix = new ARTMatrix(parseFloatArray(tmp2));
                }
                tmp2 = tmp[4].Split(' ');
                if (tmp2.Length >= 1)
                {
                    if (numberOfButtons > 0)
                    {
                        buttonStates = parseInt(tmp2[0]);
                        controllerStates = new float[tmp2.Length - 1];
                        for (int i = 0; i < controllerStates.Length; i++)
                        {
                            controllerStates[i] = parseFloat(tmp2[(i + 1)]);
                        }
                    }
                    else
                    {
                        controllerStates = new float[tmp2.Length];
                        for (int i = 0; i < controllerStates.Length; i++)
                        {
                            controllerStates[i] = parseFloat(tmp2[i]);
                        }
                    }
                }
                addFlystick(new ARTFlystick(id, visible, numberOfButtons,
                    buttonStates, numberOfControllers, controllerStates,
                    position, matrix));
                for (int i = 0; i < observers.Count; i++)
                {
                    ((ARTObserver) observers[i]).onFlystickUpdate(this);
                }
                foreach (IFlystickListener listener in flystickListener)
                {
                    UpdateFlystickListener(listener);
                }
            }
        }

        protected void decodeMsg(String msg)
        {
            msg = msg.Replace("\n", "x").Replace("\r", "");
            String[] tmp = msg.Split('x');
            for (int i = 0; i < tmp.Length; i++)
            {
                if (tmp[i].Contains("fr "))
                {
                    frame = parseInt(tmp[i].Replace("fr ", ""));
                }
                else if (tmp[i].Contains("ts "))
                {
                    timestamp = parseFloat(tmp[i].Replace("ts ", ""));
                }
                else if (tmp[i].Contains("6d "))
                {
                    parse6d(tmp[i].Replace("6d ", ""));
                }
                else if (tmp[i].Contains("6df2 "))
                {
                    parse6df2(tmp[i].Replace("6df2 ", ""));
                }
            }
        }

        public void run()
        {
            var buffer = new byte[2048];
            var adr = new IPEndPoint(IPAddress.Parse(IP), port);
            running = true;
            int timeout = 0;
            try
            {
                dsocket = new UdpClient(port);
                Thr = new Thread(() =>
                {
                    while (running)
                    {
                        buffer = dsocket.Receive(ref adr);
                        rawData = Encoding.ASCII.GetString(buffer);
                        decodeMsg(rawData);
                        for (int i = 0; i < observers.Count; i++)
                        {
                            ((ARTObserver) observers[i]).onFrameUpdate(this);
                        }
                        if (rawData == "")
                        {
                            timeout++;
                        }
                        if (timeout == 100)
                        {
                            Thr.Abort();        //
                            running = false;    //THIS HAS BEEN CHANGED orig: stop();
                            throw new Exception("Client timed out. Check if DTrack is running and sending packages over " + IP + ":" + port + ".");
                        }
                    }
                });
                Thr.IsBackground = true;
                Thr.Start();
            }
            catch (Exception e)
            {
            }
        }

        public void stop()
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

        public String status()
        {
            if (Thr != null && running)
            {
                if (dsocket != null)
                {
                    return "Thread is up and running/Client alive and listening to " + port;
                }
                return "Thread is up and running/Client seems to be dead";
            }
            return "Thread nor Client running";
        }
    }
}
