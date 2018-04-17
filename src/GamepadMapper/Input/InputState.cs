using System;
using System.Collections.Generic;
using GamepadMapper.Configuration;
using XInputDotNetPure;

namespace GamepadMapper.Input
{
    public class InputState
    {
        private struct Point
        {
            public readonly double X;
            public readonly double Y;

            public Point(double x, double y)
            {
                X = x;
                Y = y;
            }
        }

        private static Point Normalize(double x, double y, double deadzone)
        {
            var dist = Math.Sqrt(x * x + y * y);
            var angle = Math.Atan2(y, x);

            if (dist <= deadzone)
            {
                return new Point();
            }

            if (dist > 1d)
            {
                dist = 1d;
            }

            dist = (dist - deadzone) / (1d - deadzone);

            return new Point(
                dist * Math.Cos(angle),
                dist * Math.Sin(angle)
            );
        }

        public static InputState FromGamePadState(GamePadState gamePadState, IDeadzoneConfiguration deadzones)
        {
            var ltDeadzone = deadzones.LtDeadzone;
            var rtDeadzone = deadzones.RtDeadzone;

            var buttons = gamePadState.Buttons;
            var keys = gamePadState.DPad;
            var triggers = gamePadState.Triggers;
            var sticks = gamePadState.ThumbSticks;
            var lpoint = Normalize(sticks.Left.X, sticks.Left.Y, deadzones.LsDeadzone);
            var rpoint = Normalize(sticks.Right.X, sticks.Right.Y, deadzones.RsDeadzone);
            var ls = new AnalogState(lpoint.X, lpoint.Y);
            var rs = new AnalogState(rpoint.X, rpoint.Y);
            var dict = new Dictionary<Button, ButtonState>
            {
                [Button.A] = new ButtonState(buttons.A == XInputDotNetPure.ButtonState.Pressed),
                [Button.B] = new ButtonState(buttons.B == XInputDotNetPure.ButtonState.Pressed),
                [Button.X] = new ButtonState(buttons.X == XInputDotNetPure.ButtonState.Pressed),
                [Button.Y] = new ButtonState(buttons.Y == XInputDotNetPure.ButtonState.Pressed),
                [Button.Back] = new ButtonState(buttons.Back == XInputDotNetPure.ButtonState.Pressed),
                [Button.Start] = new ButtonState(buttons.Start == XInputDotNetPure.ButtonState.Pressed),
                [Button.DPadLeft] = new ButtonState(keys.Left == XInputDotNetPure.ButtonState.Pressed),
                [Button.DPadUp] = new ButtonState(keys.Up == XInputDotNetPure.ButtonState.Pressed),
                [Button.DPadRight] = new ButtonState(keys.Right == XInputDotNetPure.ButtonState.Pressed),
                [Button.DPadDown] = new ButtonState(keys.Down == XInputDotNetPure.ButtonState.Pressed),
                [Button.Lb] = new ButtonState(buttons.LeftShoulder == XInputDotNetPure.ButtonState.Pressed),
                [Button.Rb] = new ButtonState(buttons.RightShoulder == XInputDotNetPure.ButtonState.Pressed),
                [Button.Lt] = new ButtonState(triggers.Left > ltDeadzone, Math.Min(1d, (triggers.Left - ltDeadzone) / (1d - ltDeadzone))),
                [Button.Rt] = new ButtonState(triggers.Right > rtDeadzone, Math.Min(1d, (triggers.Right - rtDeadzone) / (1d - rtDeadzone))),
                [Button.Lsb] = new ButtonState(buttons.LeftStick == XInputDotNetPure.ButtonState.Pressed),
                [Button.Rsb] = new ButtonState(buttons.RightStick == XInputDotNetPure.ButtonState.Pressed)
            };

            return new InputState(dict, ls, rs);
        }

        public InputState(IReadOnlyDictionary<Button, ButtonState> buttonStates, AnalogState leftAnalogState, AnalogState rightAnalogState)
        {
            ButtonStates = buttonStates;
            LeftAnalogState = leftAnalogState;
            RightAnalogState = rightAnalogState;
        }

        public IReadOnlyDictionary<Button, ButtonState> ButtonStates { get; }

        public AnalogState LeftAnalogState { get; }

        public AnalogState RightAnalogState { get; }
    }
}