using System;
using System.Collections.Generic;
using XInputDotNetPure;

namespace GamepadMapper.Input
{
    public class InputState
    {
        public static InputState FromGamePadState(GamePadState gamePadState, double ltDeadzone, double rtDeadzone, double lsDeadzone, double rsDeadzone)
        {
            var buttons = gamePadState.Buttons;
            var keys = gamePadState.DPad;
            var triggers = gamePadState.Triggers;
            var sticks = gamePadState.ThumbSticks;
            double lx = sticks.Left.X;
            double ly = sticks.Left.Y;
            var lDist = Math.Sqrt(lx * lx + ly * ly);
            if (lDist <= lsDeadzone)
            {
                lx = 0d;
                ly = 0d;
            }
            else
            {
                var factor = (lDist - lsDeadzone) / (1d - lsDeadzone);
                lx *= factor;
                ly *= factor;
            }

            double rx = sticks.Right.X;
            double ry = sticks.Right.Y;
            var rDist = Math.Sqrt(rx * rx + ry * ry);
            if (rDist <= rsDeadzone)
            {
                rx = 0d;
                ry = 0d;
            }
            else
            {
                var factor = (rDist - rsDeadzone) / (1d - rsDeadzone);
                rx *= factor;
                ry *= factor;
            }

            var ls = new AnalogState(lx, ly);
            var rs = new AnalogState(rx, ry);
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