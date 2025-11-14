using System;
using System.Globalization;
using System.Windows.Data;

namespace Ban_Sach_Online.Converters
{
    public class StockStatusConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int soLuong = (int)value;
            return soLuong > 0 ? $"Còn {soLuong} sản phẩm" : "Hết hàng";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
