using System;
using System.Linq;
using System.Windows;
using System.Windows.Media.Imaging;
using Ban_Sach_Online.Data;
using Ban_Sach_Online.Models;
using System.Data.Entity;
using System.Windows.Media;

namespace Ban_Sach_Online.Views.KhachHang
{
    public partial class ChiTietSach_KH : Window
    {
        private readonly CSDL_Context db;
        private Sach sach;
        private int khachHangId;

        public ChiTietSach_KH(int maSach, int khachHangId)
        {
            InitializeComponent();
            db = new CSDL_Context();
            this.khachHangId = khachHangId;
            LoadThongTinSach(maSach);
        }

        private void LoadThongTinSach(int maSach)
        {
            // 🔍 Lấy sách từ CSDL (đã được Admin cập nhật)
            sach = db.Sachs
                     .Include(s => s.TheLoai)
                     .Include(s => s.AnhSachs)
                     .FirstOrDefault(s => s.SachId == maSach);

            if (sach == null)
            {
                MessageBox.Show("Không tìm thấy sách này.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                this.Close();
                return;
            }

            // --- Hiển thị dữ liệu ---
            txtTenSach.Text = sach.TenSach;
            txtTacGia.Text = sach.TacGia;
            txtNhaXB.Text = sach.NhaXB;
            txtNamXB.Text = sach.NamXB.ToString();
            txtTheLoai.Text = sach.TheLoai?.TenTheLoai ?? "Không rõ";
            txtGia.Text = $"{sach.Gia:N0} đ";
            txtSoLuongTon.Text = sach.SoLuong.ToString();
            txtMoTa.Text = string.IsNullOrEmpty(sach.MoTa) ? "Chưa có mô tả" : sach.MoTa;

            // --- Ảnh sách ---
            var duongDanAnh = sach.AnhSachs.FirstOrDefault()?.Url ?? "Views/KhachHang/no_image.png";
            try
            {
                imgSach.Source = new BitmapImage(new Uri($"pack://siteoforigin:,,,/{duongDanAnh}"));
            }
            catch
            {
                imgSach.Source = null;
            }
        }
        private void btnThemGioHang_Click(object sender, RoutedEventArgs e)
        {
            if (sach == null) return;

            // 🔍 Kiểm tra số lượng tồn
            if (sach.SoLuong <= 0)
            {
                btnThemGioHang.IsEnabled = false;
                btnMuaNgay.IsEnabled = false;
                txtSoLuongTon.Text = "Hết hàng";
                txtSoLuongTon.Foreground = Brushes.Red;

                MessageBox.Show("Sách này hiện đã hết hàng, không thể thêm vào giỏ.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            else
            {
                txtSoLuongTon.Text = sach.SoLuong.ToString();
                txtSoLuongTon.Foreground = Brushes.Black;
            }
            var gioHang = db.Set<Ban_Sach_Online.Models.GioHang>()
                            .Include(g => g.ChiTietGioHangs)
                            .FirstOrDefault(g => g.GioHangId == khachHangId)
                            ?? DangNhap.KhachHangHienTai.GioHang;

            if (gioHang == null)
            {
                gioHang = new Ban_Sach_Online.Models.GioHang { GioHangId = khachHangId };
                db.GioHangs.Add(gioHang);
                db.SaveChanges();
            }

            // 🔹 Kiểm tra sách đã có trong giỏ hay chưa
            var chiTiet = gioHang.ChiTietGioHangs.FirstOrDefault(c => c.SachId == sach.SachId);
            if (chiTiet != null)
            {
                chiTiet.SoLuong += 1;
            }
            else
            {
                chiTiet = new ChiTietGioHang
                {
                    GioHangId = gioHang.GioHangId,
                    SachId = sach.SachId,
                    SoLuong = 1
                };
                db.ChiTietGioHangs.Add(chiTiet);
            }

            // 🛒 Nếu còn hàng, cho phép thêm vào giỏ
            db.SaveChanges();
            CartWindow.ThemSachVaoGio(sach);
            MessageBox.Show($"Đã thêm '{sach.TenSach}' vào giỏ hàng!", "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);
        }


        private void btnMuaNgay_Click(object sender, RoutedEventArgs e)
        {
            if (sach == null) return;

            // 💳 Giả lập mua ngay (chuyển sang trang ThanhToan nếu có)
            MessageBox.Show($"Bạn đã chọn mua ngay '{sach.TenSach}'.", "Xác nhận", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
