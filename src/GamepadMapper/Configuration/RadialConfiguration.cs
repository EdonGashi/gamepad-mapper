namespace GamepadMapper.Configuration
{
    public interface IRadialConfiguration
    {
        double MinRadius { get; }

        double Smoothing { get; }

        bool InvertX { get; }

        bool InvertY { get; }
    }

    public class RadialConfiguration : IRadialConfiguration
    {
        public double MinRadius { get; set; } = 0.5d;

        public double Smoothing { get; set; } = 100d;

        public bool InvertX { get; set; } = false;

        public bool InvertY { get; set; } = false;
    }
}
