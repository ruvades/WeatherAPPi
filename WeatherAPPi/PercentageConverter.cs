using System;
using System.Globalization;
using System.Windows.Data;

namespace WeatherAPPi
{
    public class PercentageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double actualWidth && parameter is string percentageString)
            {
                if (double.TryParse(percentageString, out double percentage))
                {
                    return actualWidth * percentage;
                }
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
