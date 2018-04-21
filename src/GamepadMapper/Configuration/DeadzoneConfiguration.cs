namespace GamepadMapper.Configuration
{
    public interface IDeadzoneConfiguration
    {
        double Lt { get; }

        double Rt { get; }

        double Ls { get; }

        double Rs { get; }
    }

    public class DeadzoneConfiguration : IDeadzoneConfiguration
    {
        public double Lt { get; set; } = 0.1d;

        public double Rt { get; set; } = 0.1d;

        public double Ls { get; set; } = 0.1d;

        public double Rs { get; set; } = 0.1d;
    }
}