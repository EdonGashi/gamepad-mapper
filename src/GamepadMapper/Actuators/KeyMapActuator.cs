using WindowsInput;
using WindowsInput.Native;

namespace GamepadMapper.Actuators
{
    public class KeyMapActuator : IMapping
    {
        public KeyMapActuator(IKeyboardSimulator keyboard, IMouseSimulator mouse,  VirtualKeyCode virtualKey)
        {
            Keyboard = keyboard;
            Mouse = mouse;
            VirtualKey = virtualKey;
        }

        public IKeyboardSimulator Keyboard { get; }

        public IMouseSimulator Mouse { get; }

        public VirtualKeyCode VirtualKey { get; }

        public void Activate()
        {
            switch (VirtualKey)
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
                    Keyboard.KeyDown(VirtualKey);
                    break;
            }
        }

        public void Deactivate()
        {
            switch (VirtualKey)
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
                    Keyboard.KeyUp(VirtualKey);
                    break;
            }
        }
    }
}
