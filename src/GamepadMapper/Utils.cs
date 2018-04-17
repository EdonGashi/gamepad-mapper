using System.Collections.Generic;
using WindowsInput;
using WindowsInput.Native;
using GamepadMapper.Actuators;
using GamepadMapper.Configuration;
using GamepadMapper.Handlers;
using GamepadMapper.Input;
using GamepadMapper.Menu;
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

        public static Profile GetDefaultProfile(IMenuController menuController)
        {
            var simulator = new InputSimulator();

            IButtonHandler MapKey(VirtualKeyCode key)
            {
                return new KeyMapHandler(new KeyMapActuator(simulator.Keyboard, simulator.Mouse, key));
            }

            IButtonHandler PressKey(VirtualKeyCode key, ModifierKeys mods = 0)
            {
                return new KeyPressHandler(new KeyPressActuator(simulator.Keyboard, key, mods));
            }

            IButtonHandler RepeatKey(VirtualKeyCode key, ModifierKeys mods = 0)
            {
                return new KeyPressRepeatHandler(new KeyPressActuator(simulator.Keyboard, key, mods), new RepeatConfiguration());
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
            //var scrollHandler = new MovementHandler(new MouseScrollActuator(simulator.Mouse), new MovementConfiguration());
            var scrollHandler = new RadialHandler(new MenuPointerActuator(menuController), new RadialConfiguration());

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
                [InputKey.Lb] = new KeyMapHandler(new MenuVisibilityActuator(menuController)),
                [InputKey.Lt] = RepeatKey(VirtualKeyCode.BACK),
                [InputKey.Rt] = RepeatKey(VirtualKeyCode.SPACE),
                [InputKey.Back] = PressHold(VirtualKeyCode.TAB, ModifierKeys.Control | ModifierKeys.Alt, VirtualKeyCode.TAB, ModifierKeys.Alt),
                [InputKey.Start] = PressHold(VirtualKeyCode.LWIN, 0, VirtualKeyCode.VK_D, ModifierKeys.WinKey),
                [InputKey.Lsb] = MapKey(VirtualKeyCode.MBUTTON),
                [InputKey.Rsb] = PressKey(VirtualKeyCode.HOME),

                //// Mod
                [InputKey.ModA] = MapKey(VirtualKeyCode.LBUTTON),
                [InputKey.ModB] = RepeatKey(VirtualKeyCode.DELETE),
                [InputKey.ModX] = PressHold(VirtualKeyCode.TAB, ModifierKeys.Control, VirtualKeyCode.TAB, ModifierKeys.Control | ModifierKeys.Shift),
                [InputKey.ModY] = PressHold(VirtualKeyCode.VK_T, ModifierKeys.Control, VirtualKeyCode.VK_W, ModifierKeys.Control),
                [InputKey.ModDPadLeft] = RepeatKey(VirtualKeyCode.LEFT, ModifierKeys.Alt),
                [InputKey.ModDPadUp] = RepeatKey(VirtualKeyCode.VOLUME_UP),
                [InputKey.ModDPadRight] = RepeatKey(VirtualKeyCode.RIGHT, ModifierKeys.Alt),
                [InputKey.ModDPadDown] = RepeatKey(VirtualKeyCode.VOLUME_DOWN),
                [InputKey.ModLt] = RepeatKey(VirtualKeyCode.VK_Z, ModifierKeys.Control),
                [InputKey.ModRt] = RepeatKey(VirtualKeyCode.VK_Y, ModifierKeys.Control),
                [InputKey.ModBack] = PressHold(VirtualKeyCode.VK_F, 0, VirtualKeyCode.BROWSER_REFRESH, 0),
                [InputKey.ModStart] = PressHold(VirtualKeyCode.VK_E, ModifierKeys.WinKey, VirtualKeyCode.ESCAPE, ModifierKeys.Shift | ModifierKeys.Control),
                [InputKey.ModLsb] = MapKey(VirtualKeyCode.MBUTTON),
                [InputKey.ModRsb] = PressKey(VirtualKeyCode.END)
            };

            return new Profile("Default", modifiers, mouseHandler, scrollHandler, buttonHandlers);
        }
    }
}
