namespace GamepadMapper.Actuators
{
    public interface IClearable
    {
        void Clear();
    }

    public interface IAction
    {
        void Execute();
    }

    public interface IMapping
    {
        void Activate();

        void Deactivate();
    }

    public interface IMovementActuator
    {
        void Move(double x, double y);
    }
}
