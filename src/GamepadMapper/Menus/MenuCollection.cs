using System;
using System.Collections.Generic;

namespace GamepadMapper.Menus
{
    public class MenuCollection
    {
        public MenuCollection(IDictionary<string, Menu> menus)
        {
            var dict = new Dictionary<string, Menu>(StringComparer.OrdinalIgnoreCase);
            foreach (var pair in menus)
            {
                dict[pair.Key] = pair.Value;
            }

            Menus = dict;
        }

        public IReadOnlyDictionary<string, Menu> Menus { get; }
    }
}
