namespace GamepadMapper.Configuration
{
    public interface IHoldConfiguration
    {
        double Duration { get; }
    }

    public class HoldConfiguration : IHoldConfiguration
    {
        public double Duration { get; set; } = 800d;
    }
}