using System;
using System.Linq;
using System.Windows;
using Ban_Sach_Online.Data;
using System.Data.Entity;
using Ban_Sach_Online.Models;

namespace Ban_Sach_Online.Views.Admin
{
    public partial class LichSuMuaHang : Window
    {
        private readonly CSDL_Context _context;
        private readonly int _khachHangId; // Id khách hàng cần xem

        public LichSuMuaHang(int khachHangId)
        {
            InitializeComponent();
            _context = new CSDL_Context();
            _khachHangId = khachHangId;
            LoadHoaDon();
        }

        private void LoadHoaDon()
        {
            // ✅ Lấy dữ liệu từ DB trước, convert sau khi đã về bộ nhớ
            var dsHoaDon = _context.HoaDons
                .Where(h => h.KhachHangId == _khachHangId)
                .ToList() // lấy dữ liệu trước
                .Select(h => new
                {
                    HoaDonId = h.HoaDonId, // cần dùng để lấy chi tiết
                    NgayLap = h.NgayLap.ToString("dd/MM/yyyy"),
                    TongTien = h.TongTien,
                    TrangThai = h.TrangThai
                })
                .ToList();

            dgHoaDon.ItemsSource = dsHoaDon;
            dgChiTiet.ItemsSource = null;
        }

        private void DgHoaDon_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (dgHoaDon.SelectedItem == null) return;

            // Lấy ID hóa đơn
            var selected = dgHoaDon.SelectedItem;
            int hoaDonId = (int)selected.GetType().GetProperty("HoaDonId").GetValue(selected);

            // Lấy chi tiết hóa đơn
            var chiTiet = _context.ChiTietHoaDons
                .Include(c => c.Sach)
                .Where(c => c.HoaDonId == hoaDonId)
                .Select(c => new
                {
                    c.Sach.TenSach,
                    c.SoLuong,
                    c.DonGia,
                    ThanhTien = c.SoLuong * c.DonGia
                })
                .ToList();

            dgChiTiet.ItemsSource = chiTiet;
        }
    }
}
