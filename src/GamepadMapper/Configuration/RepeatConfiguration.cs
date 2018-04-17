namespace GamepadMapper.Configuration
{
    public interface IRepeatConfiguration
    {
        double DelayMilliseconds { get; }

        double RepeatMilliseconds { get; }

        double PressureAcceleration { get; }
    }

    public class RepeatConfiguration : IRepeatConfiguration
    {
        public double DelayMilliseconds { get; set; } = 600d;

        public double RepeatMilliseconds { get; } = 50d;

        public double PressureAcceleration { get; } = 1d;
    }
}
