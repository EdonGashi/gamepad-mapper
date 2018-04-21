namespace GamepadMapper.Configuration
{
    public interface IRepeatConfiguration
    {
        double Delay { get; }

        double Interval { get; }

        double PressureAcceleration { get; }
    }

    public class RepeatConfiguration : IRepeatConfiguration
    {
        public double Delay { get; set; } = 600d;

        public double Interval { get; set; } = 50d;

        public double PressureAcceleration { get; set; } = 1d;
    }
}
