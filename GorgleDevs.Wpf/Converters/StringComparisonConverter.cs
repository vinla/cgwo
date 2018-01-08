using System;
using System.Globalization;
using System.Windows.Data;

namespace GorgleDevs.Wpf.Converters
{
    public class StringComparisonConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return parameter == null;

            var valueAsString = value.ToString();

            return valueAsString.Equals(parameter);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
