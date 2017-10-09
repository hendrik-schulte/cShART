namespace cShART
{
    public interface IFlystickListener
    {
        void OnMiddleButtonPress();

        void OnLeftButtonPress();

        void OnRightButtonPress();

        void OnTriggerPress();

        /// <summary>
        /// For rudimentary stick movement.
        /// </summary>
        void OnStickUp();

        void OnStickDown();

        void OnStickLeft();

        void OnStickRight();
    }
}