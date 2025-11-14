using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Ban_Sach_Online.Data;
using Ban_Sach_Online.Models;

namespace Ban_Sach_Online.Views.KhachHang
{
    public partial class ThanhToanWindow : Window
    {
        private readonly List<ChiTietGioHang> _danhSachChiTiet;
        private readonly decimal _tongTien;

        public ThanhToanWindow(List<ChiTietGioHang> danhSachChiTiet, decimal tongTien)
        {
            InitializeComponent();
            _danhSachChiTiet = danhSachChiTiet;
            _tongTien = tongTien;

            // Hiển thị thông tin tổng tiền lên UI
            txtTongTien.Text = $"{_tongTien:N0} đ";

            // Nếu bạn có ListView để hiển thị sản phẩm
            if (listSanPham != null)
                listSanPham.ItemsSource = _danhSachChiTiet;
        }

        private void BtnHuy_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        private void BtnXacNhan_Click(object sender, RoutedEventArgs e)
        {
            string hoTen = txtHoTen.Text.Trim();
            string soDienThoai = txtSoDienThoai.Text.Trim();
            string diaChi = txtDiaChi.Text.Trim();
            string phuongThuc = (cbPhuongThuc.SelectedItem as ComboBoxItem)?.Content?.ToString();

            // 🔹 Kiểm tra dữ liệu nhập
            if (string.IsNullOrEmpty(hoTen) || string.IsNullOrEmpty(soDienThoai) || string.IsNullOrEmpty(diaChi))
            {
                MessageBox.Show("Vui lòng điền đầy đủ thông tin nhận hàng.", "Thiếu thông tin",
                                MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var khachHang = DangNhap.KhachHangHienTai;
            if (khachHang == null)
            {
                MessageBox.Show("Bạn cần đăng nhập để mua hàng!", "Cảnh báo",
                                MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                using (var db = new CSDL_Context())
                {
                    // 🔹 Tạo hóa đơn mới
                    var hoaDon = new HoaDon
                    {
                        NgayLap = DateTime.Now,
                        TongTien = _tongTien,
                        TrangThai = "Chờ xử lý",
                        KhachHangId = khachHang.KhachHangId,
                        DiaChiGiaoHang = diaChi
                    };

                    //Thêm hóa đơn trước để lấy ID
                    db.HoaDons.Add(hoaDon);
                    db.SaveChanges();

                    //Sau đó thêm chi tiết hóa đơn
                    foreach (var item in _danhSachChiTiet)
                    {
                        db.ChiTietHoaDons.Add(new ChiTietHoaDon
                        {
                            HoaDonId = hoaDon.HoaDonId, // Giờ ID đã có
                            SachId = item.Sach.SachId,
                            SoLuong = item.SoLuong,
                            DonGia = item.Sach.Gia
                        });
                    }
                    db.SaveChanges();
                    var gioHang = db.GioHangs
                        .Include("ChiTietGioHangs")
                        .FirstOrDefault(g => g.GioHangId == khachHang.KhachHangId);

                    if (gioHang != null)
                    {
                        var daMuaIds = _danhSachChiTiet.Select(c => c.Sach.SachId).ToList();
                        var chiTietCanXoa = gioHang.ChiTietGioHangs
                            .Where(c => daMuaIds.Contains(c.Sach.SachId))
                            .ToList();

                        db.ChiTietGioHangs.RemoveRange(chiTietCanXoa);
                        db.SaveChanges();
                    }
                    foreach (var item in _danhSachChiTiet)
                    {
                        var vm = CartWindow.GioHangHienTai.ChiTietGioHangs
                            .FirstOrDefault(c => c.Sach.SachId == item.Sach.SachId);
                        if (vm != null)
                            CartWindow.GioHangHienTai.ChiTietGioHangs.Remove(vm);
                    }
                }

                MessageBox.Show($"Đơn hàng đã được đặt thành công!\nPhương thức thanh toán: {phuongThuc}",
                                "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);

                this.DialogResult = true; // Thông báo cho CartWindow biết là thanh toán xong
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tạo hóa đơn: {ex.Message}", "Lỗi",
                                MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
