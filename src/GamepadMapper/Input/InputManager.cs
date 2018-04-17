using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GamepadMapper.Configuration;
using XInputDotNetPure;

namespace GamepadMapper.Input
{
    public class InputManager
    {
        public async Task Run(IDeadzoneConfiguration configuration, Profile initialProfile, double fps)
        {
            var pollTime = TimeSpan.FromMilliseconds(1000d);
            var frameTime = 1000d / fps;
            var profileStack = new Stack<Profile>();
            profileStack.Push(initialProfile);

            var connected = false;
            var isEnabled = true;
            var lastBack = false;
            var lastStart = false;
            var lastFrame = DateTime.Now;
            var skipCycles = 0;
            var dirtyButtons = new HashSet<Button>();
            while (true)
            {
                var frameStart = DateTime.Now;
                var profile = profileStack.Peek();

                // Try read input.
                if (!TryGetLowestIndex(out var state, out var playerIndex))
                {
                    if (connected)
                    {
                        // If we were connected, clear state to prevent stuck keys.
                        profile.ClearState();
                        connected = false;
                    }

                    // We don't need to check FPS times for a new gamepad.
                    await Task.Delay(pollTime);
                    continue;
                }

                if (!connected)
                {
                    // Skip this cycle because tDelta may be inconsistent.
                    skipCycles = 1;
                }

                connected = true;

                var inputState = InputState.FromGamePadState(state, configuration);
                if (dirtyButtons.Count != 0)
                {
                    // Clear dirty buttons.
                    foreach (var pair in inputState.ButtonStates)
                    {
                        if (pair.Value.IsPressed)
                        {
                            continue;
                        }

                        if (dirtyButtons.Remove(pair.Key))
                        {
                            if (dirtyButtons.Count == 0)
                            {
                                break;
                            }
                        }
                    }
                }

                var back = state.Buttons.Back == XInputDotNetPure.ButtonState.Pressed;
                var start = state.Buttons.Start == XInputDotNetPure.ButtonState.Pressed;
                if (back && start)
                {
                    if (!lastBack || !lastStart)
                    {
                        isEnabled = !isEnabled;
                        // Ignore profile for 0.2 seconds after toggling to prevent accidental clicks.
                        dirtyButtons.Add(Button.Back);
                        dirtyButtons.Add(Button.Start);
                        profile.ClearState();
                    }
                }

                lastBack = back;
                lastStart = start;

                if (isEnabled)
                {
                    if (skipCycles > 0)
                    {
                        skipCycles--;
                    }
                    else
                    {
                        var frame = new FrameDetails(playerIndex, profile, frameStart, fps, frameTime, (frameStart - lastFrame).TotalMilliseconds, inputState);
                        UpdateProfile(profile, inputState, frame, dirtyButtons);
                    }
                }

                if (false /* TODO: profile changed */)
                {
                    dirtyButtons.Clear();
                    foreach (var pair in inputState.ButtonStates)
                    {
                        if (pair.Value.IsPressed)
                        {
                            dirtyButtons.Add(pair.Key);
                        }
                    }
                }

                lastFrame = frameStart;
                var frameDuration = (DateTime.Now - frameStart).TotalMilliseconds;
                if (frameTime > frameDuration)
                {
                    // Try to keep a constant frame rate.
                    await Task.Delay(TimeSpan.FromMilliseconds(frameTime - frameDuration));
                }
            }
        }

        private static void UpdateProfile(Profile profile, InputState inputState, FrameDetails frame, HashSet<Button> dirtyButtons)
        {
            const int modA = (int)InputKey.ModA;
            var isModifierDown = false;
            foreach (var modifierKey in profile.Modifiers)
            {
                if (inputState.ButtonStates[modifierKey].IsPressed)
                {
                    isModifierDown = true;
                    break;
                }
            }

            profile.LeftAnalogHandler?.Update(inputState.LeftAnalogState, ThumbStick.Left, frame);
            profile.RightAnalogHandler?.Update(inputState.RightAnalogState, ThumbStick.Right, frame);
            var nonPressedKey = new ButtonState();
            // Some enum hacking is done below...
            // Button is converted to InputKey 1 to 1, and for converting to Mod<Button> it's added by an offset.
            foreach (var pair in inputState.ButtonStates)
            {
                if (profile.Modifiers.Contains(pair.Key))
                {
                    // Modifiers are ignored.
                    continue;
                }

                if (dirtyButtons.Contains(pair.Key))
                {
                    continue;
                }

                var button = (InputKey)pair.Key;
                var modButton = (InputKey)(modA + (int)pair.Key);
                if (profile.ButtonHandlers.TryGetValue(button, out var buttonHandler))
                {
                    buttonHandler?.Update(isModifierDown ? nonPressedKey : pair.Value, button, frame);
                }

                if (profile.ButtonHandlers.TryGetValue(modButton, out var modButtonHandler))
                {
                    modButtonHandler?.Update(isModifierDown ? pair.Value : nonPressedKey, modButton, frame);
                }
            }
        }

        private static bool TryGetLowestIndex(out GamePadState gamePadState, out PlayerIndex playerIndex)
        {
            gamePadState = GamePad.GetState(PlayerIndex.One, GamePadDeadZone.None);
            playerIndex = PlayerIndex.One;
            if (gamePadState.IsConnected)
            {
                return true;
            }

            gamePadState = GamePad.GetState(PlayerIndex.Two, GamePadDeadZone.None);
            playerIndex = PlayerIndex.Two;
            if (gamePadState.IsConnected)
            {
                return true;
            }

            gamePadState = GamePad.GetState(PlayerIndex.Three, GamePadDeadZone.None);
            playerIndex = PlayerIndex.Three;
            if (gamePadState.IsConnected)
            {
                return true;
            }

            gamePadState = GamePad.GetState(PlayerIndex.Four, GamePadDeadZone.None);
            playerIndex = PlayerIndex.Four;
            if (gamePadState.IsConnected)
            {
                return true;
            }

            return false;
        }
    }
}