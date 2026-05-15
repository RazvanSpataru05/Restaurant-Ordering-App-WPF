using System.Globalization;
using System.Windows.Data;

namespace RestaurantOrderingApp.Converters
{
    public class StringTrimConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
                => value?.ToString().Trim() ?? string.Empty;
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}
