using System.Collections.Generic;
using System.Linq;
using WindowsInput;
using WindowsInput.Native;
using GamepadMapper.Input;

namespace GamepadMapper.Actuators
{
    public class KeyPressActuator : IAction
    {
        private readonly IReadOnlyList<VirtualKeyCode> modifiersList;

        public KeyPressActuator(IKeyboardSimulator keyboard, IMouseSimulator mouse, VirtualKeyCode key)
            : this(keyboard, mouse, new[] { key }, 0)
        {
        }

        public KeyPressActuator(IKeyboardSimulator keyboard, IMouseSimulator mouse, VirtualKeyCode key, ModifierKeys modifiers)
            : this(keyboard, mouse, new[] { key }, modifiers)
        {
        }

        public KeyPressActuator(IKeyboardSimulator keyboard, IMouseSimulator mouse, IEnumerable<VirtualKeyCode> keys)
            : this(keyboard, mouse, keys, 0)
        {
        }

        public KeyPressActuator(IKeyboardSimulator keyboard, IMouseSimulator mouse, IEnumerable<VirtualKeyCode> keys, ModifierKeys modifiers)
        {
            Keyboard = keyboard;
            Mouse = mouse;
            var list = new List<VirtualKeyCode>();
            foreach (var key in keys ?? Enumerable.Empty<VirtualKeyCode>())
            {
                switch (key)
                {
                    case VirtualKeyCode.MENU:
                        modifiers |= ModifierKeys.Alt;
                        break;
                    case VirtualKeyCode.CONTROL:
                        modifiers |= ModifierKeys.Control;
                        break;
                    case VirtualKeyCode.SHIFT:
                        modifiers |= ModifierKeys.Shift;
                        break;
                    case VirtualKeyCode.LWIN:
                        modifiers |= ModifierKeys.WinKey;
                        break;
                    default:
                        list.Add(key);
                        break;
                }
            }

            Keys = list;
            modifiersList = Utils.ModifiersToKeys(modifiers);
            Modifiers = modifiers;
        }

        public IKeyboardSimulator Keyboard { get; }

        public IMouseSimulator Mouse { get; }

        public ModifierKeys Modifiers { get; }

        public IReadOnlyList<VirtualKeyCode> Keys { get; }

        public void Execute()
        {
            foreach (var modifier in modifiersList)
            {
                Keyboard.KeyDown(modifier);
            }

            foreach (var key in Keys)
            {
                switch (key)
                {
                    case VirtualKeyCode.LBUTTON:
                        Mouse.LeftButtonClick();
                        break;
                    case VirtualKeyCode.MBUTTON:
                        Mouse.MiddleButtonClick();
                        break;
                    case VirtualKeyCode.RBUTTON:
                        Mouse.RightButtonClick();
                        break;
                    default:
                        Keyboard.KeyPress(key);
                        break;
                }
            }

            foreach (var modifier in modifiersList.Reverse())
            {
                Keyboard.KeyUp(modifier);
            }
        }
    }
}
