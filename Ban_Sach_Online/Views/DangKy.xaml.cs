using System;
using System.Linq;
using System.Windows;
using Ban_Sach_Online.Data;
using Ban_Sach_Online.Models;

namespace Ban_Sach_Online.Views
{
    public partial class DangKy : Window
    {
        private readonly CSDL_Context _context;

        public DangKy()
        {
            InitializeComponent();
            _context = new CSDL_Context();
        }

        private void BtnDangKy_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string hoTen = txtHoTen.Text.Trim();
                string soDienThoai = txtSoDienThoai.Text.Trim();
                string tenDangNhap = txtTenDangNhap.Text.Trim();
                string matKhau = txtMatKhau.Password.Trim();
                string xacNhanMatKhau = txtXacNhanMatKhau.Password.Trim();
                string email = txtEmail.Text.Trim();

                // Kiểm tra dữ liệu rỗng
                if (string.IsNullOrEmpty(hoTen) ||
                    string.IsNullOrEmpty(soDienThoai) ||
                    string.IsNullOrEmpty(tenDangNhap) ||
                    string.IsNullOrEmpty(matKhau) ||
                    string.IsNullOrEmpty(xacNhanMatKhau) ||
                    string.IsNullOrEmpty(email))
                {
                    MessageBox.Show("Vui lòng điền đầy đủ thông tin!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Kiểm tra mật khẩu trùng khớp
                if (matKhau != xacNhanMatKhau)
                {
                    MessageBox.Show("Mật khẩu xác nhận không khớp!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Kiểm tra email trùng
                if (_context.KhachHangs.Any(k => k.Email == email))
                {
                    MessageBox.Show("Email này đã được sử dụng!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Kiểm tra số điện thoại hợp lệ
                if (!soDienThoai.All(char.IsDigit) || soDienThoai.Length < 9)
                {
                    MessageBox.Show("Số điện thoại không hợp lệ!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Tạo khách hàng mới
                var khachHang = new Ban_Sach_Online.Models.KhachHang
                {
                    HoTen = hoTen,
                    Email = email,
                    MatKhau = matKhau,
                    SoDienThoai = soDienThoai,
                    DiaChi = "",
                    NgayDangKy = DateTime.Now,
                    GioHang = new GioHang()
                };

                _context.KhachHangs.Add(khachHang);
                _context.SaveChanges();

                MessageBox.Show("Đăng ký thành công!", "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);

                // Mở lại màn hình đăng nhập
                var dangNhap = new DangNhap();
                dangNhap.Show();
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đã xảy ra lỗi khi đăng ký: " + ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnHuy_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
