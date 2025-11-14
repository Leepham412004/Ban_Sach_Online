using System.Linq;
using System.Windows;
using Ban_Sach_Online.Data;

namespace Ban_Sach_Online.Views.Admin
{
    public partial class QlNguoiDung : Window
    {
        private readonly CSDL_Context _context;

        public QlNguoiDung()
        {
            InitializeComponent();
            _context = new CSDL_Context();
            LoadNguoiDung();
        }

        private void LoadNguoiDung()
        {
            dgNguoiDung.ItemsSource = _context.KhachHangs
                .Select(k => new
                {
                    k.KhachHangId,
                    k.HoTen,
                    k.Email,
                    k.SoDienThoai,
                    k.NgayDangKy
                })
                .ToList();
        }

        private void btnTimKiem_Click(object sender, RoutedEventArgs e)
        {
            string tuKhoa = txtTimKiem.Text.Trim().ToLower();

            var ketQua = _context.KhachHangs
                .Where(k => k.HoTen.ToLower().Contains(tuKhoa)
                         || k.Email.ToLower().Contains(tuKhoa))
                .Select(k => new
                {
                    k.KhachHangId,
                    k.HoTen,
                    k.Email,
                    k.SoDienThoai,
                    k.NgayDangKy
                })
                .ToList();

            dgNguoiDung.ItemsSource = ketQua;
        }
        private void txtTimKiem_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            string tuKhoa = txtTimKiem.Text.Trim().ToLower();
            var ketQua = _context.KhachHangs
                .Where(k => k.HoTen.ToLower().Contains(tuKhoa)
                         || k.Email.ToLower().Contains(tuKhoa))
                .Select(k => new
                {
                    k.KhachHangId,
                    k.HoTen,
                    k.Email,
                    k.SoDienThoai,
                })
                .ToList();

            dgNguoiDung.ItemsSource = ketQua;
        }

        private void btnXemLichSu_Click(object sender, RoutedEventArgs e)
        {
            if (dgNguoiDung.SelectedItem == null)
            {
                MessageBox.Show("Vui lòng chọn một khách hàng để xem lịch sử mua hàng!",
                                "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var selected = dgNguoiDung.SelectedItem;
            var maKh = (int)selected.GetType().GetProperty("KhachHangId").GetValue(selected);

            var lichSu = new LichSuMuaHang(maKh);
            lichSu.ShowDialog();
        }
    }
}
