using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using Ban_Sach_Online.Models;
using Ban_Sach_Online.Data;
using System.Data.Entity;

namespace Ban_Sach_Online.Views.KhachHang
{
    public partial class TaiKhoan : Window
    {
        private readonly CSDL_Context _context = new CSDL_Context();

        public string TenTaiKhoan { get; set; }
        public ObservableCollection<DonHangViewModel> DanhSachDonHang { get; set; }
        public int TongDonHang => DanhSachDonHang?.Count ?? 0;

        public TaiKhoan(Ban_Sach_Online.Models.KhachHang khachHang)
        {
            InitializeComponent();

            TenTaiKhoan = khachHang?.HoTen ?? "Khách hàng";

            // 🔹 Lấy danh sách đơn hàng có include dữ liệu liên quan
            var danhSach = _context.HoaDons
                .Include(h => h.ChiTietHoaDons.Select(ct => ct.Sach))
                .Where(h => h.KhachHangId == khachHang.KhachHangId)
                .SelectMany(h => h.ChiTietHoaDons.Select(ct => new DonHangViewModel
                {
                    MaDon = h.HoaDonId.ToString(),
                    TenSach = ct.Sach.TenSach,
                    SoLuong = ct.SoLuong,
                    TongTien = ct.SoLuong * ct.DonGia,
                    NgayDat = h.NgayLap,
                    TinhTrang = h.TrangThai
                }))
                .ToList();

            DanhSachDonHang = new ObservableCollection<DonHangViewModel>(danhSach);

            DataContext = this;
        }

        private void DangXuat_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Bạn có chắc muốn đăng xuất không?", "Đăng xuất",
                MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                var loginWindow = new DangNhap();
                loginWindow.Show();

                foreach (Window win in Application.Current.Windows)
                {
                    if (win != loginWindow)
                        win.Close();
                }
            }
        }
    }

    public class DonHangViewModel
    {
        public string MaDon { get; set; }
        public string TenSach { get; set; }
        public int SoLuong { get; set; }
        public decimal TongTien { get; set; }
        public DateTime NgayDat { get; set; }
        public string TinhTrang { get; set; }

        public Brush MauTinhTrang
        {
            get
            {
                switch (TinhTrang)
                {
                    case "Đang xử lý":
                        return Brushes.Orange;
                    case "Đang giao":
                        return Brushes.DeepSkyBlue;
                    case "Đã giao":
                        return Brushes.Green;
                    default:
                        return Brushes.Gray;
                }
            }
        }
    }
}
