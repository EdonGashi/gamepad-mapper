using System;
using XInputDotNetPure;

namespace GamepadMapper.Input
{
    public struct FrameDetails
    {
        public FrameDetails(PlayerIndex playerIndex, Profile profile, DateTime time, double timeDelta, double fps, InputState inputState, bool isConnected)
        {
            PlayerIndex = playerIndex;
            Profile = profile;
            Time = time;
            TimeDelta = timeDelta;
            Fps = fps;
            InputState = inputState;
            IsConnected = isConnected;
        }

        public PlayerIndex PlayerIndex { get; }

        public Profile Profile { get; }

        public DateTime Time { get; }

        public double TimeDelta { get; }

        public double Fps { get; }

        public double FrameTime => 1000d / Fps;

        public InputState InputState { get; }

        public bool IsConnected { get; }
    }
}