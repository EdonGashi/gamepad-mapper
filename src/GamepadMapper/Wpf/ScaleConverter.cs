using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using GamepadMapper.Configuration;

namespace GamepadMapper.Wpf
{
    public class ScaleConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(values[0] is MenuPosition position) || !(values[1] is double scale) || !(parameter is string s))
            {
                return DependencyProperty.UnsetValue;
            }

            switch (s.ToLower())
            {
                case "viewbox.width":
                    return scale * (position.IsCenter() ? 1080d : 720d);
                case "viewbox.height":
                    return scale * (position.IsMiddle() ? 1080d : 720d);
                case "viewbox.margin":
                    var left = position.IsLeft() ? 60d : 0d;
                    var top = position.IsTop() ? 60d : 0d;
                    var right = position.IsRight() ? 60d : 0d;
                    var bottom = position.IsBottom() ? 60d : 0d;
                    return new Thickness(left, top, right, bottom);
                default:
                    return DependencyProperty.UnsetValue;
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return new[] { Binding.DoNothing };
        }
    }
}
