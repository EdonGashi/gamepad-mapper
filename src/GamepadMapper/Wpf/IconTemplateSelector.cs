using System.Windows;
using System.Windows.Controls;
using GamepadMapper.Menus;

namespace GamepadMapper.Wpf
{
    public class IconTemplateSelector : DataTemplateSelector
    {
        public DataTemplate MaterialIcon { get; set; }
        public DataTemplate TextIcon { get; set; }
        public DataTemplate CompositeIcon { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            switch (item)
            {
                case MaterialIcon _:
                    return MaterialIcon;
                case TextIcon _:
                    return TextIcon;
                case CompositeIcon _:
                    return CompositeIcon;
                default:
                    return base.SelectTemplate(item, container);
            }
        }
    }
}
