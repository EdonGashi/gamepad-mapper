using System.Collections.Generic;
using System.Linq;

namespace GamepadMapper.Configuration
{
    public class ProfileConfiguration
    {
        public ProfileConfiguration(string name, IEnumerable<InputBinding> bindings)
        {
            Name = name;
            Bindings = bindings?.ToArray() ?? new InputBinding[0];
        }

        public string Name { get; }

        public IEnumerable<InputBinding> Bindings { get; }
    }
}
