using System.Collections.Generic;
using System.Linq;
using WindowsInput.Native;

namespace GamepadMapper.Configuration
{
    public abstract class ActionDescriptor
    {
        protected static string Escape(string value)
        {
            return value.Replace("\"", "\"\"");
        }

        public override string ToString() => Stringify();

        public abstract string Stringify();
    }

    public class CommandAction : ActionDescriptor
    {
        public CommandAction(string command)
        {
            Command = command;
        }

        public string Command { get; }

        public override string Stringify() => $"command(\"{Escape(Command)}\")";
    }

    public class KeyAction : ActionDescriptor
    {
        public KeyAction(IEnumerable<VirtualKeyCode> keys)
        {
            Keys = keys?.ToArray() ?? new VirtualKeyCode[0];
        }

        public IEnumerable<VirtualKeyCode> Keys { get; }

        public override string Stringify() => string.Join("+", Keys);
    }

    public class Macro : ActionDescriptor
    {
        public Macro(IEnumerable<ActionDescriptor> actions)
        {
            Actions = actions;
        }

        public IEnumerable<ActionDescriptor> Actions { get; }

        public override string Stringify() => string.Join(";", Actions);
    }

    public class ShowMenuAction : ActionDescriptor
    {
        public ShowMenuAction(string menu)
        {
            Menu = menu;
        }

        public string Menu { get; }

        public override string Stringify() => $"show(\"{Escape(Menu)}\")";
    }

    public class RunProgramAction : ActionDescriptor
    {
        public RunProgramAction(string path, string arguments)
        {
            Path = path;
            Arguments = arguments;
        }

        public string Path { get; }

        public string Arguments { get; }

        public override string Stringify() => $"run(\"{Escape(Path)}\"" + (Arguments != null
            ? $",\"{Escape(Arguments)}\")"
            : ")");
    }

    public class FlashConfigurationAction : ActionDescriptor
    {
        public FlashConfigurationAction(string key)
        {
            Key = key;
        }

        public string Key { get; }

        public override string Stringify() => $"flashcfg(\"{Escape(Key)}\")";
    }

    public class FlashMessageAction : ActionDescriptor
    {
        public FlashMessageAction(string title, string message, string modifier)
        {
            Title = title;
            Message = message;
            Modifier = modifier;
        }

        public string Title { get; }

        public string Message { get; }

        public string Modifier { get; }

        public override string Stringify() => $"flashmsg(\"{Escape(Title)}\",\"{Escape(Message)}\",\"{Escape(Modifier)}\")";
    }

    public class NoOpAction : ActionDescriptor
    {
        public override string Stringify() => "nothing";
    }

    public class IncrementConfigurationAction : ActionDescriptor
    {
        public IncrementConfigurationAction(string key)
        {
            Key = key;
        }

        public string Key { get; }

        public override string Stringify() => $"increment(\"{Escape(Key)}\")";
    }

    public class DecrementConfigurationAction : ActionDescriptor
    {
        public DecrementConfigurationAction(string key)
        {
            Key = key;
        }

        public string Key { get; }

        public override string Stringify() => $"decrement(\"{Escape(Key)}\")";
    }

    public class ToggleConfigurationAction : ActionDescriptor
    {
        public ToggleConfigurationAction(string key)
        {
            Key = key;
        }

        public string Key { get; }

        public override string Stringify() => $"toggle(\"{Escape(Key)}\")";
    }

    public class ResetConfigurationAction : ActionDescriptor
    {
        public ResetConfigurationAction(string key)
        {
            Key = key;
        }

        public string Key { get; }

        public override string Stringify() => $"reset(\"{Escape(Key)}\")";
    }

    public class SetConfigurationAction : ActionDescriptor
    {
        public SetConfigurationAction(string key, string value)
        {
            Key = key;
            Value = value;
        }

        public string Key { get; }

        public string Value { get; }

        public override string Stringify() => $"set(\"{Escape(Key)}\", \"{Escape(Value)}\")";
    }
}