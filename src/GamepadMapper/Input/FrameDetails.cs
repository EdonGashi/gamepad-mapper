using System;
using XInputDotNetPure;

namespace GamepadMapper.Input
{
    public struct FrameDetails
    {
        public FrameDetails(PlayerIndex playerIndex, Profile profile, DateTime time, double fps, double timeDelta, InputState inputState)
        {
            PlayerIndex = playerIndex;
            Profile = profile;
            Time = time;
            Fps = fps;
            TimeDelta = timeDelta;
            InputState = inputState;
        }

        public PlayerIndex PlayerIndex { get; }

        public Profile Profile { get; }

        public DateTime Time { get; }

        public double Fps { get; }

        public double TimeDelta { get; }
        
        public InputState InputState { get; }
    }
}