using System.Collections.Generic;
using System.Linq;
using GamepadMapper.Configuration.Parsing;

namespace GamepadMapper.Configuration
{
    public class MenuConfiguration
    {
        public MenuConfiguration(
            string name,
            HelpConfiguration help,
            IEnumerable<CommandBinding> commandBindings,
            IEnumerable<PageConfiguration> pages)
        {
            Name = name;
            Help = help;
            CommandBindings = commandBindings?.ToList() ?? new List<CommandBinding>(0);
            Pages = pages?.ToList() ?? new List<PageConfiguration>(0);
        }

        public string Name { get; }

        public HelpConfiguration Help { get; }

        public IReadOnlyList<CommandBinding> CommandBindings { get; }

        public IReadOnlyList<PageConfiguration> Pages { get; }
    }

    public class PageConfiguration
    {
        public PageConfiguration(HelpConfiguration help, IEnumerable<MenuItemConfiguration> items)
        {
            Help = help;
            Items = items?.ToList() ?? new List<MenuItemConfiguration>(0);
        }

        public HelpConfiguration Help { get; }

        public IReadOnlyList<MenuItemConfiguration> Items { get; }
    }

    public class HelpConfiguration
    {
        public HelpConfiguration(string helpText, IEnumerable<InputHelpText> inputHelpTexts)
        {
            HelpText = helpText;
            InputHelpTexts = inputHelpTexts?.ToList() ?? new List<InputHelpText>(0);
        }

        public string HelpText { get; }

        public IReadOnlyList<InputHelpText> InputHelpTexts { get; }
    }

    public class InputHelpText
    {
        public InputHelpText(HelpKey key, string description)
        {
            Key = key;
            Description = description;
        }

        public HelpKey Key { get; }

        public string Description { get; }
    }

    public class MenuItemConfiguration
    {
        public MenuItemConfiguration(
            string name,
            IEnumerable<Token> icon,
            string text,
            IEnumerable<CommandBinding> commandBindings,
            HelpConfiguration help)
        {
            Name = name;
            Icon = icon?.ToList() ?? new List<Token>(0);
            Text = text;
            CommandBindings = commandBindings?.ToList() ?? new List<CommandBinding>(0);
            Help = help;
        }

        public string Name { get; }

        public IReadOnlyList<Token> Icon { get; }

        public string Text { get; }

        public IReadOnlyList<CommandBinding> CommandBindings { get; }

        public HelpConfiguration Help { get; }
    }
}