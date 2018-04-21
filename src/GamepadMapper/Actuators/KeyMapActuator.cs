using System.Collections.Generic;
using System.Linq;
using WindowsInput;
using WindowsInput.Native;

namespace GamepadMapper.Actuators
{
    public class KeyMapActuator : IMapping, IClearable
    {
        public KeyMapActuator(IKeyboardSimulator keyboard, IMouseSimulator mouse, VirtualKeyCode key)
            : this(keyboard, mouse, new[] { key })
        {
        }

        public KeyMapActuator(IKeyboardSimulator keyboard, IMouseSimulator mouse, IEnumerable<VirtualKeyCode> virtualKeys)
        {
            Keyboard = keyboard;
            Mouse = mouse;
            VirtualKeys = virtualKeys?.ToList() ?? new List<VirtualKeyCode>(0);
        }

        public IKeyboardSimulator Keyboard { get; }

        public IMouseSimulator Mouse { get; }

        public IReadOnlyList<VirtualKeyCode> VirtualKeys { get; }

        public void Activate()
        {
            foreach (var key in VirtualKeys)
            {
                switch (key)
                {
                    case VirtualKeyCode.LBUTTON:
                        Mouse.LeftButtonDown();
                        break;
                    case VirtualKeyCode.MBUTTON:
                        Mouse.MiddleButtonDown();
                        break;
                    case VirtualKeyCode.RBUTTON:
                        Mouse.RightButtonDown();
                        break;
                    default:
                        Keyboard.KeyDown(key);
                        break;
                }
            }
        }

        public void Deactivate()
        {
            foreach (var key in VirtualKeys.Reverse())
            {
                switch (key)
                {
                    case VirtualKeyCode.LBUTTON:
                        Mouse.LeftButtonUp();
                        break;
                    case VirtualKeyCode.MBUTTON:
                        Mouse.MiddleButtonUp();
                        break;
                    case VirtualKeyCode.RBUTTON:
                        Mouse.RightButtonUp();
                        break;
                    default:
                        Keyboard.KeyUp(key);
                        break;
                }
            }
        }

        public void Clear()
        {
            Deactivate();
        }
    }
}
