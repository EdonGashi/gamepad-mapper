namespace GamepadMapper.Configuration
{
    public interface IRadialConfiguration
    {
        double MinRadius { get; }

        double SmoothingMilliseconds { get; }

        bool InvertX { get; }

        bool InvertY { get; }
    }

    public class RadialConfiguration : IRadialConfiguration
    {
        public double MinRadius { get; } = 0.5d;

        public double SmoothingMilliseconds { get; set; } = 150d;

        public bool InvertX { get; set; } = false;

        public bool InvertY { get; set; } = false;
    }
}
