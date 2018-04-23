using System;
using System.Globalization;
using System.Windows.Data;

namespace GamepadMapper.Wpf
{
    public class SizeSelectorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return 120d;
            }

            return 80d;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }
}
