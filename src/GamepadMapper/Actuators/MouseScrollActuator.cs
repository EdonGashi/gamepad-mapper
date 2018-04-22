using System;
using WindowsInput;

namespace GamepadMapper.Actuators
{
    public class MouseScrollActuator : IMovementActuator
    {
        public MouseScrollActuator(IMouseSimulator mouseSimulator)
        {
            MouseSimulator = mouseSimulator;
        }

        public IMouseSimulator MouseSimulator { get; }

        public void Move(double x, double y)
        {
            MouseSimulator.VerticalScroll(-(int)Math.Round(y));
            MouseSimulator.HorizontalScroll((int)Math.Round(x));
        }
    }
}
