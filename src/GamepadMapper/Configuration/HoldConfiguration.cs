namespace GamepadMapper.Configuration
{
    public interface IHoldConfiguration
    {
        double HoldMilliseconds { get; }
    }

    public class HoldConfiguration : IHoldConfiguration
    {
        public double HoldMilliseconds { get; set; } = 600d;
    }
}