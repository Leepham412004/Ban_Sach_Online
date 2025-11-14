using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace Ban_Sach_Online.Views.KhachHang.Converters
{
    // ✅ Converter hiển thị trạng thái tồn kho
    public class StockStatusConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int soLuong)
            {
                return soLuong > 0 ? $"Còn {soLuong} sản phẩm" : "Hết hàng";
            }
            return "Không xác định";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    // ✅ Converter đổi màu chữ theo trạng thái tồn kho
    public class StockColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int soLuong)
            {
                return soLuong > 0 ? Brushes.Green : Brushes.Red;
            }
            return Brushes.Black;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
