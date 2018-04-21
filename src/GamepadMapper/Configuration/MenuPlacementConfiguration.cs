namespace GamepadMapper.Configuration
{
    public class MenuPlacementConfiguration
    {
        public double Scale { get; set; } = 1d;

        public MenuPosition MenuPosition { get; set; } = MenuPosition.BottomRight;
    }

    public enum MenuPosition
    {
        TopLeft,
        TopCenter,
        TopRight,
        MiddleLeft,
        MiddleCenter,
        MiddleRight,
        BottomLeft,
        BottomCenter,
        BottomRight,
        Fill
    }
}
