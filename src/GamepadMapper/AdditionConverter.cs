using System;
using System.Globalization;
using System.Windows.Data;

namespace GamepadMapper
{
    public class AdditionConverter : IValueConverter
    {
        public double Value { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double d)
            {
                return d + Value;
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double d)
            {
                return d - Value;
            }

            return value;
        }
    }
}
