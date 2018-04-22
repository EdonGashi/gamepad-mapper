using WindowsInput;

namespace GamepadMapper.Actuators
{
    public class SendStringActuator : IAction
    {
        public SendStringActuator(string str, IKeyboardSimulator keyboard)
        {
            String = str;
            Keyboard = keyboard;
        }

        public string String { get; }

        public IKeyboardSimulator Keyboard { get; }

        public void Execute()
        {
            Keyboard.TextEntry(String);
        }
    }
}
