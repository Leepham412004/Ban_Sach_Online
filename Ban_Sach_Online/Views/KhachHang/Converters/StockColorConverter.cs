using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace Ban_Sach_Online.Converters
{
    public class StockColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int soLuong = (int)value;
            return soLuong > 0 ? Brushes.Green : Brushes.Red;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
