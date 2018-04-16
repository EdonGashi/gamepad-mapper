using System;

namespace GamepadMapper.Input
{
    public interface IHandler
    {
        void ClearState();
    }

    public interface IAnalogHandler : IHandler
    {
        void Update(AnalogState state, ThumbStick thumbStick, FrameDetails frameDetails);
    }

    public interface IButtonHandler : IHandler
    {
        void Update(ButtonState state, InputKey inputKey, FrameDetails frameDetails);
    }
}
