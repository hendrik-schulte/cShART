namespace cShART
{
    public interface IFlystickListener
    {
        void onMiddleButtonPress(ARTFlystick flystick);

        void onLeftButtonPress(ARTFlystick flystick);

        void onRightButtonPress(ARTFlystick flystick);

        void onTriggerPress(ARTFlystick flystick);

        /// <summary>
        /// For rudimentary stick movement.
        /// </summary>
        void onStickUp(ARTFlystick flystick);

        void onStickDown(ARTFlystick flystick);

        void onStickLeft(ARTFlystick flystick);

        void onStickRight(ARTFlystick flystick);

    }
}
