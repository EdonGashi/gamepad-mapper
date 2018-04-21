using System.Collections.Generic;
using System.Linq;
using GamepadMapper.Configuration;
using GamepadMapper.Infrastructure;

namespace GamepadMapper.Menus
{
    public class Menu
    {
        public static Menu FromConfig(MenuConfiguration config, IActionFactory actionFactory)
        {
            return new Menu(
                CommandBindingCollection.FromCollection(config.CommandBindings, actionFactory),
                config.Pages.Select((p, i) => MenuPage.FromConfig(p, i, actionFactory)),
                config.Help);
        }

        public Menu(CommandBindingCollection commandBindings, IEnumerable<MenuPage> pages, HelpConfiguration helpScreen)
        {
            CommandBindings = commandBindings;
            var pagesList = pages?.ToList() ?? new List<MenuPage>();
            if (pagesList.Count == 0)
            {
                pagesList.Add(new MenuPage(0, null, null));
            }

            Pages = pagesList;
            HelpScreen = helpScreen;
        }

        public CommandBindingCollection CommandBindings { get; }

        public IReadOnlyList<MenuPage> Pages { get; }

        public HelpConfiguration HelpScreen { get; }
    }

    public class MenuPage
    {
        public static MenuPage FromConfig(PageConfiguration config, int index, IActionFactory actionFactory)
        {
            return new MenuPage(
                index,
                config.Help,
                config.Items.Select((item, i) => PageItem.FromConfig(item, i, config.Items.Count, actionFactory)));
        }

        public MenuPage(int index, HelpConfiguration helpScreen, IEnumerable<PageItem> items)
        {
            Index = index;
            HelpScreen = helpScreen;
            Items = items?.ToList() ?? new List<PageItem>();
        }

        public int Index { get; }

        public HelpConfiguration HelpScreen { get; }

        public IReadOnlyList<PageItem> Items { get; }
    }

    public class PageItem
    {
        public const double AngleStart = 0d;

        public static PageItem FromConfig(MenuItemConfiguration config, int index, int totalItems, IActionFactory actionFactory)
        {
            return new PageItem(index, totalItems,
                 CommandBindingCollection.FromCollection(config.CommandBindings, actionFactory),
                 config.Help,
                 null,
                 config.Name,
                 config.Text);
        }

        private static double Normalize(double angle)
        {
            return (angle + 360d) % 360d;
        }

        public PageItem(
            int index,
            int totalItems,
            CommandBindingCollection commandBindings,
            HelpConfiguration helpScreen,
            object icon,
            string title,
            string description)
        {
            Index = index;
            var angleStep = 360d / totalItems;
            var angleStart = AngleStart + index * angleStep;
            Angle = angleStart;
            StartAngle = Normalize(angleStart - angleStep / 2d);
            EndAngle = Normalize(angleStart + angleStep / 2d);
            CommandBindings = commandBindings;
            HelpScreen = helpScreen;
            Icon = icon;
            Title = title;
            Description = description;
        }

        public int Index { get; }

        public double Angle { get; }

        public double StartAngle { get; }

        public double EndAngle { get; }

        public object Icon { get; }

        public string Title { get; }

        public string Description { get; }

        public CommandBindingCollection CommandBindings { get; }

        public HelpConfiguration HelpScreen { get; }
    }
}
