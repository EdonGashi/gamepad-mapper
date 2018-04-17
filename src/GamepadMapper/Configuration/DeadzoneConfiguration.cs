namespace GamepadMapper.Configuration
{
    public interface IDeadzoneConfiguration
    {
        double LtDeadzone { get; }

        double RtDeadzone { get; }

        double LsDeadzone { get; }

        double RsDeadzone { get; }
    }

    public class DeadzoneConfiguration : IDeadzoneConfiguration
    {
        public double LtDeadzone { get; set; } = 0.1d;

        public double RtDeadzone { get; set; } = 0.1d;

        public double LsDeadzone { get; set; } = 0.1d;

        public double RsDeadzone { get; set; } = 0.1d;
    }
}