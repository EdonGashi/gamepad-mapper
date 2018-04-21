using GamepadMapper.Configuration;
using GamepadMapper.Input;

namespace GamepadMapper.Handlers
{
    public interface IHandler
    {
        void ClearState();
    }

    public interface IAnalogHandler : IHandler
    {
        void Update(AnalogState state, ThumbStick thumbStick, FrameDetails frame);
    }

    public interface IButtonHandler : IHandler
    {
        void Update(ButtonState state, InputKey inputKey, FrameDetails frame);
    }
}
