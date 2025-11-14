using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Ban_Sach_Online.Data;
using Ban_Sach_Online.Models;
using System.Data.Entity;
using System.Collections.ObjectModel;
using ModelsGioHang = Ban_Sach_Online.Models.GioHang;
using UIGioHang = Ban_Sach_Online.Views.KhachHang.GioHang;
using Ban_Sach_Online.Views.KhachHang;

namespace Ban_Sach_Online.Views
{
    public partial class DangNhap : Window
    {
        private readonly CSDL_Context _context;

        public DangNhap()
        {
            InitializeComponent();
            _context = new CSDL_Context();
        }

        // Lưu thông tin khách hàng hiện tại (có thể null nếu chưa đăng nhập)
        public static Ban_Sach_Online.Models.KhachHang KhachHangHienTai { get; private set; }
        public static bool LaAdmin { get; private set; } = false;

        // Nút Đăng Nhập
        private void BtnDangNhap_Click(object sender, RoutedEventArgs e)
        {
            string taiKhoan = txtEmail.Text.Trim();
            string matKhau = txtMatKhau.Password.Trim();

            if (string.IsNullOrEmpty(taiKhoan) || string.IsNullOrEmpty(matKhau))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ Tài khoản/Email và Mật khẩu!", "Cảnh báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Đăng nhập admin
            if ((taiKhoan.Equals("admin@gmail.com", StringComparison.OrdinalIgnoreCase) || taiKhoan.Equals("admin", StringComparison.OrdinalIgnoreCase))
                && matKhau == "123456")
            {
                LaAdmin = true;
                MessageBox.Show("Đăng nhập với quyền Admin thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                new Views.Admin.AdminWindow().Show();
                this.Close();
                return;
            }

            // Kiểm tra khách hàng
            var khachHang = _context.KhachHangs
                .FirstOrDefault(u =>
                    (u.Email.Equals(taiKhoan, StringComparison.OrdinalIgnoreCase) ||
                     u.TenDangNhap.Equals(taiKhoan, StringComparison.OrdinalIgnoreCase))
                    && u.MatKhau == matKhau);

            if (khachHang == null)
            {
                MessageBox.Show("Sai thông tin đăng nhập! Vui lòng kiểm tra lại.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Lưu khách hàng hiện tại
            KhachHangHienTai = khachHang;
            LaAdmin = false;

            // Lấy giỏ hàng từ DB (ModelsGioHang)
            ModelsGioHang gioHangDb = _context.GioHangs
                .Include(g => g.ChiTietGioHangs.Select(c => c.Sach))
                .FirstOrDefault(g => g.GioHangId == khachHang.KhachHangId);

            if (gioHangDb == null)
            {
                gioHangDb = new ModelsGioHang
                {
                    GioHangId = khachHang.KhachHangId
                };
                _context.GioHangs.Add(gioHangDb);
                _context.SaveChanges();
            }

            // Chuyển dữ liệu sang giỏ hàng UI (UIGioHang)
            CartWindow.GioHangHienTai = new UIGioHang
            {
                ChiTietGioHangs = new ObservableCollection<ChiTietGioHang>(gioHangDb.ChiTietGioHangs)
            };

            // Mở trang chủ
            new Views.KhachHang.TrangChuWindow(khachHang).Show();
            this.Close();
        }


        // Nút Đăng Ký
        private void BtnDangKy_Click(object sender, RoutedEventArgs e)
        {
            var dangKyWindow = new DangKy();
            dangKyWindow.Show();
            this.Close();
        }

        // Nút Quên Mật Khẩu
        private void BtnQuenMatKhau_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Chức năng quên mật khẩu đang được phát triển.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
