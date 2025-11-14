using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Media.Imaging;
using Ban_Sach_Online.Models;
using Ban_Sach_Online.Data;
using System.Data.Entity;

namespace Ban_Sach_Online.Views.KhachHang
{
    public partial class SuKienWindow : Window
    {
        private readonly CSDL_Context _context;
        private ObservableCollection<SachViewModel> _danhSachSach;
        private int khachHangId;

        public SuKienWindow(int khachHangId)
        {
            InitializeComponent();
            _context = new CSDL_Context();
            this.khachHangId = khachHangId;
            TaiDanhSachSach();
        }

        private void TaiDanhSachSach()
        {
            try
            {
                var sachSuKien = _context.Sachs
                    .Include(s => s.AnhSachs)
                    .Where(s => s.LaNgayDoi == true)
                    .ToList();

                // Map sang ViewModel với Bitmap
                _danhSachSach = new ObservableCollection<SachViewModel>(
                    sachSuKien.Select(s => new SachViewModel
                    {
                        SachId = s.SachId,
                        TenSach = s.TenSach,
                        TacGia = s.TacGia,
                        MoTa = s.MoTa,
                        SoLuongCon = s.SoLuong,
                        SoLuongDaBan = s.SoLuongDaBan,
                        GiaGoc = s.Gia,
                        GiaSauGiam = s.GiaGiam ?? 0,
                        PhanTramGiam = s.GiaGiam.HasValue ? (int)((s.Gia - s.GiaGiam.Value) / s.Gia * 100) : 0,
                        AnhSach = s.AnhSachs.FirstOrDefault()?.Url ?? "Views/KhachHang/no_image.png"
                    }).ToList()
                );

                // Thêm Bitmap trực tiếp
                foreach (var sach in _danhSachSach)
                {
                    sach.HinhAnhBitmap = LoadBitmap(sach.AnhSach);
                }

                DanhSachSach.ItemsSource = _danhSachSach;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải danh sách sách: " + ex.Message);
            }
        }

        // Hàm load Bitmap
        private BitmapImage LoadBitmap(string relativePath)
        {
            try
            {
                string fullPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, relativePath);
                if (!System.IO.File.Exists(fullPath))
                    fullPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Views/KhachHang/no_image.png");

                var bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(fullPath, UriKind.Absolute);
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.EndInit();
                return bitmap;
            }
            catch
            {
                return null;
            }
        }

        private void Sach_Click(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var border = sender as System.Windows.Controls.Border;
            var sach = border?.DataContext as SachViewModel; // bind ViewModel
            if (sach == null) return;

            ChiTietSach_KH chiTiet = new ChiTietSach_KH(sach.SachId, khachHangId);
            chiTiet.ShowDialog();
        }
    }
}
