using System.Collections.Generic;
using System.Windows.Controls;
using WindowsInput;
using WindowsInput.Native;
using GamepadMapper.Actuators;
using GamepadMapper.Configuration;
using GamepadMapper.Handlers;
using GamepadMapper.Input;
using Button = GamepadMapper.Input.Button;

namespace GamepadMapper
{
    internal class Utils
    {
        public static List<VirtualKeyCode> ModifiersToKeys(ModifierKeys keys)
        {
            var list = new List<VirtualKeyCode>(3);
            if (keys.HasFlag(ModifierKeys.Control))
            {
                list.Add(VirtualKeyCode.CONTROL);
            }

            if (keys.HasFlag(ModifierKeys.Alt))
            {
                list.Add(VirtualKeyCode.MENU);
            }

            if (keys.HasFlag(ModifierKeys.Shift))
            {
                list.Add(VirtualKeyCode.SHIFT);
            }

            if (keys.HasFlag(ModifierKeys.WinKey))
            {
                list.Add(VirtualKeyCode.LWIN);
            }

            return list;
        }

        public static Profile GetDefaultProfile()
        {
            var simulator = new InputSimulator();

            IButtonHandler MapKey(VirtualKeyCode key)
            {
                return new KeyMapHandler(new KeyMapActuator(simulator.Keyboard, simulator.Mouse, key));
            }

            IButtonHandler PressKey(VirtualKeyCode key)
            {
                return new KeyPressHandler(new KeyPressActuator(simulator.Keyboard, key));
            }

            IButtonHandler RepeatKey(VirtualKeyCode key)
            {
                return new KeyPressRepeatHandler(new KeyPressActuator(simulator.Keyboard, key), new RepeatConfiguration());
            }

            IButtonHandler PressHold(
                VirtualKeyCode pressKey, ModifierKeys pressModifiers,
                VirtualKeyCode holdKey, ModifierKeys holdModifiers)
            {
                return new KeyPressHoldHandler(
                     new KeyPressActuator(simulator.Keyboard, pressKey, pressModifiers),
                     new KeyPressActuator(simulator.Keyboard, holdKey, holdModifiers),
                     new HoldConfiguration()
                );
            }

            var modifiers = new[] { Button.Rb };

            var mouseHandler = new MovementHandler(new MouseMovementActuator(simulator.Mouse), new MovementConfiguration());
            var scrollHandler = new MovementHandler(new MouseScrollActuator(simulator.Mouse), new MovementConfiguration());

            var buttonHandlers = new Dictionary<InputKey, IButtonHandler>
            {
                // Non-mod
                [InputKey.A] = MapKey(VirtualKeyCode.LBUTTON),
                [InputKey.B] = PressHold(VirtualKeyCode.ESCAPE, 0, VirtualKeyCode.F4, ModifierKeys.Alt),
                [InputKey.X] = RepeatKey(VirtualKeyCode.RETURN),
                [InputKey.Y] = MapKey(VirtualKeyCode.RBUTTON),
                [InputKey.DPadLeft] = RepeatKey(VirtualKeyCode.LEFT),
                [InputKey.DPadUp] = RepeatKey(VirtualKeyCode.UP),
                [InputKey.DPadRight] = RepeatKey(VirtualKeyCode.RIGHT),
                [InputKey.DPadDown] = RepeatKey(VirtualKeyCode.DOWN),
                [InputKey.Lt] = RepeatKey(VirtualKeyCode.BACK),
                [InputKey.Rt] = RepeatKey(VirtualKeyCode.SPACE),
                [InputKey.Back] = PressHold(VirtualKeyCode.TAB, ModifierKeys.Control | ModifierKeys.Alt, VirtualKeyCode.TAB, ModifierKeys.Alt),
                [InputKey.Start] = PressHold(VirtualKeyCode.LWIN, 0, VirtualKeyCode.VK_D, ModifierKeys.WinKey),
                [InputKey.Lsb] = MapKey(VirtualKeyCode.MBUTTON),
                [InputKey.Rsb] = PressKey(VirtualKeyCode.HOME),

                //// Mod
            };

            return new Profile("Default", modifiers, mouseHandler, scrollHandler, buttonHandlers);
        }
    }
}
