namespace GamepadMapper.Configuration
{
    public interface IMovementConfiguration
    {
        double Speed { get; }

        double Acceleration { get; }

        bool InvertX { get; }

        bool InvertY { get; }
    }

    public class MovementConfiguration : IMovementConfiguration
    {
        public double Speed { get; set; } = 400d;

        public double Acceleration { get; set; } = 1.3d;

        public bool InvertX { get; set; } = false;

        public bool InvertY { get; set; } = false;
    }
}