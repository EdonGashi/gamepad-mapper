namespace GamepadMapper.Input
{
    public struct ButtonState
    {
        public ButtonState(bool isPressed)
            : this(isPressed, isPressed ? 1d : 0d)
        {
        }

        public ButtonState(bool isPressed, double pressure)
        {
            IsPressed = isPressed;
            Pressure = pressure;
        }

        public bool IsPressed { get; }

        public double Pressure { get; }
    }
}