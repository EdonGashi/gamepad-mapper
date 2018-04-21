using System.Collections.Generic;
using System.Linq;
using GamepadMapper.Configuration;
using GamepadMapper.Handlers;

namespace GamepadMapper.Input
{
    public class Profile
    {
        public Profile(
            string name,
            IEnumerable<Button> modifiers,
            IAnalogHandler leftAnalogHandler,
            IAnalogHandler rightAnalogHandler,
            IDictionary<InputKey, IButtonHandler> buttonHandlers)
        {
            Name = name;
            Modifiers = modifiers?.ToList() ?? new List<Button>();
            LeftAnalogHandler = leftAnalogHandler;
            RightAnalogHandler = rightAnalogHandler;
            ButtonHandlers = new Dictionary<InputKey, IButtonHandler>(buttonHandlers);
        }

        public string Name { get; }

        public IReadOnlyList<Button> Modifiers { get; }

        public IAnalogHandler LeftAnalogHandler { get; }

        public IAnalogHandler RightAnalogHandler { get; }

        public IReadOnlyDictionary<InputKey, IButtonHandler> ButtonHandlers { get; }

        public void ClearState()
        {
            LeftAnalogHandler?.ClearState();
            RightAnalogHandler?.ClearState();
            foreach (var value in ButtonHandlers.Values)
            {
                value.ClearState();
            }
        }
    }
}