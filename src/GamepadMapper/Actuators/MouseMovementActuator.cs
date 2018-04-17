using System;
using WindowsInput;

namespace GamepadMapper.Actuators
{
    public class MouseMovementActuator : IMovementActuator
    {
        public MouseMovementActuator(IMouseSimulator mouseSimulator)
        {
            MouseSimulator = mouseSimulator;
        }

        public IMouseSimulator MouseSimulator { get; }

        public void Move(double x, double y)
        {
            MouseSimulator.MoveMouseBy((int)Math.Round(x), (int)Math.Round(y));
        }
    }
}
