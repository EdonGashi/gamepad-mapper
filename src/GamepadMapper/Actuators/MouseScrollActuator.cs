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
            // TODO: Input simulator does not support fine scrolling
        }
    }
}
