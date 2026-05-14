using System.Globalization;
using System.Windows.Data;

namespace RestaurantOrderingApp.Converters
{
    public class PortionQuantityToUnitConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
                     => new string(value?.ToString().Where(char.IsLetter).ToArray());

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}
