using System.Collections.Generic;
using System.Linq;
using WindowsInput;
using WindowsInput.Native;
using GamepadMapper.Input;

namespace GamepadMapper.Actuators
{
    public class KeyPressActuator : IAction
    {
        private readonly List<VirtualKeyCode> modifiersList;

        public KeyPressActuator(IKeyboardSimulator keyboard, VirtualKeyCode key)
            : this(keyboard, new[] { key }, 0)
        {
        }

        public KeyPressActuator(IKeyboardSimulator keyboard, VirtualKeyCode key, ModifierKeys modifiers)
            : this(keyboard, new[] { key }, modifiers)
        {
        }

        public KeyPressActuator(IKeyboardSimulator keyboard, IEnumerable<VirtualKeyCode> keys)
            : this(keyboard, keys, 0)
        {
        }

        public KeyPressActuator(IKeyboardSimulator keyboard, IEnumerable<VirtualKeyCode> keys, ModifierKeys modifiers)
        {
            Keyboard = keyboard;
            Keys = keys?.ToList() ?? new List<VirtualKeyCode>(0);
            modifiersList = Utils.ModifiersToKeys(modifiers);
            Modifiers = modifiers;
        }

        public IKeyboardSimulator Keyboard { get; }

        public ModifierKeys Modifiers { get; }

        public IReadOnlyList<VirtualKeyCode> Keys { get; }

        public void Execute()
        {
            Keyboard.ModifiedKeyStroke(modifiersList, Keys);
        }
    }
}
