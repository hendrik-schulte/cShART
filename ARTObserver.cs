namespace cShART
{
    public interface ARTObserver
    {
        void OnFrameUpdate(ARTClient paramARTClient);

        void On6DUpdate(ARTClient paramARTClient);

        void OnFlystickUpdate(ARTClient paramARTClient);
    }
}
