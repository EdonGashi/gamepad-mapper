using System;
using System.Globalization;
using System.Windows.Data;

namespace GamepadMapper
{
    internal class StartAngleConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(values[0] is double))
            {
                return Binding.DoNothing;
            }

            var angle = (double)values[0];
            var width = (double)values[1];
            return angle - width / 2d;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return new[] { Binding.DoNothing };
        }
    }

    internal class EndAngleConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(values[0] is double))
            {
                return Binding.DoNothing;
            }

            var angle = (double)values[0];
            var width = (double)values[1];
            return angle + width / 2d;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return new[] { Binding.DoNothing };
        }
    }
}
