using System.Windows;
using Ban_Sach_Online.Data;
using System.Linq;
using System.Windows.Controls;

namespace Ban_Sach_Online.Views.Admin
{
    public partial class QlDonHang : Window
    {
        private readonly CSDL_Context _context = new CSDL_Context();

        public QlDonHang()
        {
            InitializeComponent();
            LoadDonHang();
        }

        private void LoadDonHang()
        {
            dgDonHang.ItemsSource = _context.HoaDons
                .Where(d => d.KhachHang != null)
                .Select(d => new
                {
                    d.HoaDonId,
                    d.KhachHang.HoTen,
                    d.NgayLap,
                    d.TongTien,
                    d.TrangThai
                })
                .OrderByDescending(d => d.NgayLap)
                .ToList();
        }

        private void btnCapNhat_Click(object sender, RoutedEventArgs e)
        {
            if (dgDonHang.SelectedItem == null)
            {
                MessageBox.Show("Vui lòng chọn một đơn hàng để cập nhật!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var selected = dgDonHang.SelectedItem;
            var hoaDonId = (int)selected.GetType().GetProperty("HoaDonId").GetValue(selected);
            var trangThaiMoi = (cbTrangThai.SelectedItem as ComboBoxItem)?.Content?.ToString();

            if (string.IsNullOrEmpty(trangThaiMoi))
            {
                MessageBox.Show("Vui lòng chọn trạng thái mới!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var donHang = _context.HoaDons.FirstOrDefault(d => d.HoaDonId == hoaDonId);
            if (donHang != null)
            {
                donHang.TrangThai = trangThaiMoi;
                _context.SaveChanges();
                MessageBox.Show("Cập nhật trạng thái đơn hàng thành công!", "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);
                LoadDonHang();
            }
        }
    }
}
