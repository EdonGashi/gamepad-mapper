namespace GamepadMapper.Configuration
{
    public class CommandBinding
    {
        public CommandBinding(string command, ActionDescriptor action)
        {
            Command = command;
            Action = action;
        }

        public string Command { get; }

        public ActionDescriptor Action { get; }

        public string Stringify() => $"bind {Command} = {Action}";

        public override string ToString() => Stringify();
    }
}
