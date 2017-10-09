using System;
using System.Collections;

namespace cShART
{
    public class ARTFlystick : ARTObject
    {
        private readonly int buttonStates;
        private readonly float[] controllerStates;
        private readonly int numberOfButtons;
        private readonly int numberOfControllers;
        private readonly bool visible;

        public ARTFlystick(int id, bool visible, int numberOfButtons, int buttonStates, int numberOfControllers,
            float[] controllerStates, ARTPoint position, ARTMatrix matrix) : base(id, position, matrix)
        {
            this.visible = visible;
            this.numberOfButtons = numberOfButtons;
            this.numberOfControllers = numberOfControllers;
            this.buttonStates = buttonStates;
            this.controllerStates = controllerStates;
        }

        public static ARTFlystick Empty()
        {
            return new ARTFlystick(-1, false, 0, 0, 0, new float[0], ARTPoint.Empty(), ARTMatrix.Empty());
        }

        public bool IsVisible()
        {
            return visible;
        }

        public int GetNumberOfButtons()
        {
            return numberOfButtons;
        }

        public int GetNumberOfControllers()
        {
            return numberOfControllers;
        }

        public int GetButtonStates()
        {
            return buttonStates;
        }

        /// <summary>
        ///     This Method translates the button states integer into actual english for better handling.
        ///     (aka tells you which button is currently pushed.) Useful for debugging.
        /// </summary>
        /// <returns>Returns a string containing names of pushed buttons</returns>
        public string GetPushedButtonsByName()
        {
            //Byte binBStates = Convert.ToByte(buttonStates);
            // BitArray BA = new BitArray(binBStates);
            //int[] StatesArray = new int[]{buttonStates};
            //Array.Reverse(StatesArray);
            var binBStates = new BitArray(new[] {buttonStates});
            var output = "";
            //byte[] binBStates = BitConverter.GetBytes(buttonStates);

            if (binBStates[3])
                output = output + "LeftButton";
            if (binBStates[2])
            {
                if (!output.Equals(""))
                    output = output + "/";
                output = output + "MiddleButton";
            }
            if (binBStates[1])
            {
                if (!output.Equals(""))
                    output = output + "/";
                output = output + "RightButton";
            }
            if (binBStates[0])
            {
                if (!output.Equals(""))
                    output = output + "/";
                output = output + "Trigger";
            }
            if (output == "")
                output = "NothingPressed";
            return output + " Byte: " + binBStates;
        }

        /// <summary>
        ///     This method is for further button handling of the flystick. You will receive a bit array which represents the
        ///     currently pushed buttons.
        ///     Array value and corresponding buttons are:
        ///     [0] = Trigger
        ///     [1] = Right button
        ///     [2] = Middle button
        ///     [3] = Left button
        /// </summary>
        /// <returns>Returns a bit array represting currently pushed buttons</returns>
        public BitArray GetStateArrayOfButtons()
        {
            return new BitArray(new[] {buttonStates});
        }

        public float[] GetStickXYPos()
        {
            return controllerStates;
        }

        protected override string NameToString()
        {
            return "ARTFlystick";
        }

        protected string ControllerStatesToString()
        {
            var res = "";
            for (var i = 0; i < controllerStates.Length; i++)
            {
                res = res + controllerStates[i];
                if (i + 1 < controllerStates.Length)
                    res = res + ", ";
            }
            return res;
        }

        protected override string ExtensionsToString()
        {
            return "isVisible: " + IsVisible() + Environment.NewLine + "numberOfButtons: " + GetNumberOfButtons() +
                   ", buttonStates: " + GetButtonStates() + Environment.NewLine + "numberOfControllers: " +
                   GetNumberOfControllers() + ", controllerStates: " + ControllerStatesToString() + Environment.NewLine;
        }
    }
}