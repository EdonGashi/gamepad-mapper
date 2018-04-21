using System;
using System.Collections.Generic;
using GamepadMapper.Actuators;
using GamepadMapper.Configuration;

namespace GamepadMapper.Infrastructure
{
    public class CommandBindingCollection
    {
        public static CommandBindingCollection FromCollection(
            IEnumerable<CommandBinding> bindings,
            IActionFactory actionFactory)
        {
            var dict = new Dictionary<string, IAction>();
            foreach (var binding in bindings)
            {
                dict[binding.Command] = actionFactory.Create(binding.Action);
            }

            return new CommandBindingCollection(dict);
        }

        public CommandBindingCollection(IDictionary<string, IAction> bindings)
        {
            var dict = new Dictionary<string, IAction>(StringComparer.OrdinalIgnoreCase);
            foreach (var pair in bindings)
            {
                dict[pair.Key] = pair.Value;
            }

            Bindings = dict;
        }

        public IReadOnlyDictionary<string, IAction> Bindings { get; }

        public bool TryDispatch(string command)
        {
            if (Bindings.TryGetValue(command, out var action))
            {
                try
                {
                    action.Execute();
                }
                catch
                {
                    return true;
                }

                return true;
            }

            return false;
        }
    }
}
