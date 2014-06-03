using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cShART
{
    public class ARTFlystick : ARTObject
    {
        private bool visible;
        private int numberOfButtons;
        private int numberOfControllers;
        private int buttonStates;
        private float[] controllerStates;

        public static ARTFlystick Empty()
        {
            return new ARTFlystick(-1, false, 0, 0, 0, new float[0], ARTPoint.Empty(), ARTMatrix.Empty());
        }

        public ARTFlystick(int id, bool visible, int numberOfButtons, int buttonStates, int numberOfControllers, float[] controllerStates, ARTPoint position, ARTMatrix matrix) : base(id, position, matrix)
        {
            this.visible = visible;
            this.numberOfButtons = numberOfButtons;
            this.numberOfControllers = numberOfControllers;
            this.buttonStates = buttonStates;
            this.controllerStates = controllerStates;
        }

        public bool isVisible()
        {
            return this.visible;
        }

        public int getNumberOfButtons()
        {
            return this.numberOfButtons;
        }

        public int getNumberOfControllers()
        {
            return this.numberOfControllers;
        }

        public int getButtonStates()
        {
            return this.buttonStates;
        }

        /// <summary>
        /// This Method translates the button states integer into actual english for better handling.
        /// (aka tells you which button is currently pushed.) Useful for debugging.
        /// </summary>
        /// <returns>Returns a string containing names of pushed buttons</returns>
        public String getPushedButtonsByName()
        {
            //Byte binBStates = Convert.ToByte(buttonStates);
           // BitArray BA = new BitArray(binBStates);
            //int[] StatesArray = new int[]{buttonStates};
            //Array.Reverse(StatesArray);
            BitArray binBStates = new BitArray(new int[]{buttonStates});
            String output = "";
            //byte[] binBStates = BitConverter.GetBytes(buttonStates);

            if (binBStates[3])
            {
                output = output + "LeftButton";
            }
            if (binBStates[2])
            {
                if (!output.Equals(""))
                {
                    output = output + "/";
                }
                output = output + "MiddleButton";
            }
            if (binBStates[1])
            {
                if (!output.Equals(""))
                {
                    output = output + "/";
                }
                output = output + "RightButton";
            }
            if (binBStates[0])
            {
                if (!output.Equals(""))
                {
                    output = output + "/";
                }
                output = output + "Trigger";
            }
            if (output == "")
            {
                output = "NothingPressed";
            }
            return output + " Byte: " + binBStates.ToString();
        }

        /// <summary>
        /// This method is for further button handling of the flystick. You will receive a bit array which represents the currently pushed buttons.
        /// Array value and corresponding buttons are:
        /// [0] = Trigger
        /// [1] = Right button
        /// [2] = Middle button
        /// [3] = Left button
        /// </summary>
        /// <returns>Returns a bit array represting currently pushed buttons</returns>
        public BitArray GetStateArrayOfButtons()
        {
            return new BitArray(new int[]{buttonStates});
        }
        public float[] GetStickXYPos()
        {
            return this.controllerStates;
        }

        override protected String nameToString()
        {
            return "ARTFlystick";
        }

        protected String controllerStatesToString()
        {
            String res = "";
            for (int i = 0; i < this.controllerStates.Length; i++)
            {
                res = res + this.controllerStates[i];
                if (i + 1 < this.controllerStates.Length)
                {
                    res = res + ", ";
                }
            }
            return res;
        }

        override protected String extensionsToString()
        {
            return "isVisible: " + isVisible() + Environment.NewLine + "numberOfButtons: " + getNumberOfButtons() + ", buttonStates: " + getButtonStates() + Environment.NewLine + "numberOfControllers: " + getNumberOfControllers() + ", controllerStates: " + controllerStatesToString() + Environment.NewLine;
        }
    }
}
