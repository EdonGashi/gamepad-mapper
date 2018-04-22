using System;
using System.Globalization;
using System.Windows.Data;

namespace GamepadMapper.Wpf
{
    public class PolarToRectangularConverter : IValueConverter
    {
        public double ContainerSize { get; set; }

        public double ItemWidth { get; set; }

        public double ItemHeight { get; set; }

        public double Radius { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is double angle))
            {
                return 0d;
            }

            switch (parameter)
            {
                case "x":
                case "X":
                    return ContainerSize / 2d + Math.Cos((angle - 90d) * Math.PI / 180d) * Radius - ItemWidth / 2d;
                case "y":
                case "Y":
                    return ContainerSize / 2d + Math.Sin((angle - 90d) * Math.PI / 180d) * Radius - ItemHeight / 2d;
                default:
                    return 0d;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }
}
