using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using GamepadMapper.Configuration;
using GamepadMapper.Configuration.Parsing;
using GamepadMapper.Infrastructure;
using MahApps.Metro.IconPacks;

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

    public class MenuPage : IEnumerable<PageItem>
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

        public IEnumerator<PageItem> GetEnumerator() => Items.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    public class PageItem
    {
        public const double AngleStart = 0d;

        public static PageItem FromConfig(MenuItemConfiguration config, int index, int totalItems, IActionFactory actionFactory)
        {
            return new PageItem(index, totalItems,
                 CommandBindingCollection.FromCollection(config.CommandBindings, actionFactory),
                 config.Help,
                 IconBase.FromTokens(config.Icon),
                 config.Name,
                 config.Text);
        }

        private static double Normalize(double angle)
        {
            return (angle + 720d) % 360d;
        }

        public PageItem(
            int index,
            int totalItems,
            CommandBindingCollection commandBindings,
            HelpConfiguration helpScreen,
            IconBase icon,
            string title,
            string description)
        {
            Index = index;
            var angleStep = 360d / totalItems;
            var angleStart = AngleStart + index * angleStep;
            if (totalItems == 1)
            {
                angleStep = 180d;
            }

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

        public IconBase Icon { get; }

        public string Title { get; }

        public string Description { get; }

        public CommandBindingCollection CommandBindings { get; }

        public HelpConfiguration HelpScreen { get; }
    }

    public abstract class IconBase
    {
        public static IconBase FromTokens(IReadOnlyList<Token> tokens)
        {
            var i = 0;
            IconBase Read()
            {
                var token = tokens[i++];
                if (token.IsString)
                {
                    return new TextIcon(token.Value);
                }

                return new MaterialIcon(Enum.TryParse(token.Value, true, out PackIconMaterialKind kind)
                    ? kind
                    : PackIconMaterialKind.SquareOutline);
            }

            switch (tokens.Count)
            {
                case 0:
                    return new MaterialIcon(PackIconMaterialKind.SquareOutline);
                case 1:
                    return Read();
                case 2:
                    return new CompositeIcon(Read(), Read(), MaterialIcon.None, MaterialIcon.None);
                case 3:
                    return new CompositeIcon(Read(), Read(), Read(), MaterialIcon.None);
                default:
                    return new CompositeIcon(Read(), Read(), Read(), Read());
            }
        }
    }

    public class CompositeIcon : IconBase
    {
        public CompositeIcon(IconBase icon1, IconBase icon2, IconBase icon3, IconBase icon4)
        {
            Icon1 = icon1;
            Icon2 = icon2;
            Icon3 = icon3;
            Icon4 = icon4;
        }

        public IconBase Icon1 { get; }

        public IconBase Icon2 { get; }

        public IconBase Icon3 { get; }

        public IconBase Icon4 { get; }
    }

    public class TextIcon : IconBase
    {
        public TextIcon(string text)
        {
            Text = text;
        }

        public string Text { get; }

        public override string ToString() => Text;
    }

    public class MaterialIcon : IconBase
    {
        public static MaterialIcon None => new MaterialIcon(PackIconMaterialKind.None);

        public MaterialIcon(PackIconMaterialKind kind)
        {
            Kind = kind;
        }

        public PackIconMaterialKind Kind { get; }
    }
}
