using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace GamepadMapper.Wpf
{
    public class RadialMarginCalculator : IMultiValueConverter
    {
        public double CircleRadius { get; set; } = 180d;

        public double DesiredDistance { get; set; } = 150d;

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(values[0] is double actualWidth) || !(values[1] is double actualHeight))
            {
                return new Thickness();
            }

            if (actualWidth <= DesiredDistance)
            {
                return new Thickness(0d, 0d, DesiredDistance - actualWidth, 0d);
            }

            if (actualHeight >= CircleRadius)
            {
                return new Thickness(0d, 0d, 8d, 0d);
            }

            var distanceFromCenter = CircleRadius - actualHeight;
            var angle = Math.Atan2(distanceFromCenter, CircleRadius);
            var intrusion = CircleRadius * (1d - Math.Cos(angle));
            if (actualWidth - intrusion <= DesiredDistance)
            {
                return new Thickness(0d, 0d, DesiredDistance - actualWidth, 0d);
            }

            return new Thickness(0d, 0d, -intrusion, 0d);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return new[] { Binding.DoNothing };
        }
    }
}
