using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using GamepadMapper.Configuration;

namespace GamepadMapper.Wpf
{
    public class AlignmentConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is MenuPosition position) || !(parameter is string s))
            {
                return DependencyProperty.UnsetValue;
            }

            switch (s.ToLower())
            {
                case "h2.margin":
                    return position.IsTop() ? new Thickness(0d, 16d, 0d, 0d) : new Thickness(0d, 0d, 0d, 16d);
                case "h2.verticalalignment":
                    return position.IsTop() ? VerticalAlignment.Top : VerticalAlignment.Bottom;
                case "h2.row":
                    return position.IsTop() ? 1 : 0;
                case "h2.column":
                case "menu.column":
                    return position.IsLeft() ? 0 : 1;
                case "h.horizontalalignment":
                    return position.IsLeft() ? HorizontalAlignment.Left : HorizontalAlignment.Right;
                case "h.row":
                case "menu.row":
                    return position.IsTop() ? 0 : 1;
                case "h.column":
                    return position.IsLeft() ? 1 : 0;
                case "border.width":
                    return position.IsCenter() ? 1080d : 720d;
                case "border.height":
                    return position.IsMiddle() ? 1080d : 720d;
                case "viewbox.horizontalalignment":
                    return position.IsLeft()
                        ? HorizontalAlignment.Left
                        : position.IsCenter()
                            ? HorizontalAlignment.Center
                            : HorizontalAlignment.Right;
                case "viewbox.verticalalignment":
                    return position.IsTop()
                        ? VerticalAlignment.Top
                        : position.IsMiddle()
                            ? VerticalAlignment.Center
                            : VerticalAlignment.Bottom;
                default:
                    return DependencyProperty.UnsetValue;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
