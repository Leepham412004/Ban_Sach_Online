using Ban_Sach_Online.Data;
using Ban_Sach_Online.Models;
using System;
using System.IO;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Ban_Sach_Online.Views.KhachHang
{
    public partial class CartWindow : Window
    {
        public ObservableCollection<ChiTietGioHangViewModel> GioHangItems { get; set; }

        // Giỏ hàng dùng chung toàn app
        public static GioHang GioHangHienTai { get; set; } = new GioHang
        {
            ChiTietGioHangs = new ObservableCollection<ChiTietGioHang>()
        };

        public CartWindow()
        {
            InitializeComponent();

            GioHangItems = new ObservableCollection<ChiTietGioHangViewModel>(
                GioHangHienTai.ChiTietGioHangs.Select(c => MapToViewModel(c))
            );

            itemsGioHang.ItemsSource = GioHangItems;

            GioHangHienTai.ChiTietGioHangs.CollectionChanged += (s, e) => ReloadGioHangItems();
            foreach (var item in GioHangItems)
                item.PropertyChanged += (s, e) => TinhTongTien();

            TinhTongTien();
        }

        private void ReloadGioHangItems()
        {
            GioHangItems.Clear();
            foreach (var c in GioHangHienTai.ChiTietGioHangs)
            {
                var vm = MapToViewModel(c);
                vm.PropertyChanged += (s, e) => TinhTongTien();
                GioHangItems.Add(vm);
            }
            TinhTongTien();
        }

        // Chuyển từ Model sang ViewModel
        private static ChiTietGioHangViewModel MapToViewModel(ChiTietGioHang c)
        {
            return new ChiTietGioHangViewModel
            {
                Sach = new SachViewModel
                {
                    SachId = c.Sach.SachId,
                    TenSach = c.Sach.TenSach,
                    TacGia = c.Sach.TacGia,
                    GiaGoc = c.Sach.Gia,
                    GiaSauGiam = c.Sach.GiaGiam ?? 0,
                    SoLuongCon = c.Sach.SoLuong,
                    AnhSach = c.Sach.AnhSachs.FirstOrDefault()?.Url 
                },
                SoLuong = c.SoLuong,
                IsSelected = false
            };
        }

        // Thêm sách vào giỏ
        public static void ThemSachVaoGio(Sach sach, CartWindow cartWindow = null)
        {
            if (sach == null) return;

            var existing = GioHangHienTai.ChiTietGioHangs.FirstOrDefault(c => c.Sach.SachId == sach.SachId);

            if (existing != null)
            {
                existing.SoLuong++;
            }
            else
            {
                var chiTiet = new ChiTietGioHang { Sach = sach, SoLuong = 1 };
                GioHangHienTai.ChiTietGioHangs.Add(chiTiet);
            }

            cartWindow?.ReloadGioHangItems();

        }

        private void BtnTangSoLuong_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.DataContext is ChiTietGioHangViewModel item)
            {
                item.SoLuong++;
                TinhTongTien();
            }
        }

        private void BtnGiamSoLuong_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.DataContext is ChiTietGioHangViewModel item)
            {
                if (item.SoLuong > 1)
                {
                    item.SoLuong--;
                    TinhTongTien();
                }
            }
        }

        private void TinhTongTien()
        {
            decimal tong = GioHangItems
                .Where(c => c.IsSelected)
                .Sum(c => (decimal)(c.Sach?.GiaHienTai ?? 0) * c.SoLuong);

            txtTongTien.Text = $"{tong:N0} đ";
        }

        private void btnMuaHang_Click(object sender, RoutedEventArgs e)
        {
            var sachDaChon = GioHangItems.Where(c => c.IsSelected).ToList();
            if (!sachDaChon.Any())
            {
                MessageBox.Show("Vui lòng chọn ít nhất 1 sản phẩm để thanh toán.", "Thông báo",
                                MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            decimal tongTien = sachDaChon.Sum(c => (decimal)c.Sach.GiaHienTai * c.SoLuong);

            var danhSachChiTiet = sachDaChon.Select(c => new ChiTietGioHang
            {
                Sach = new Sach
                {
                    SachId = c.Sach.SachId,
                    TenSach = c.Sach.TenSach,
                    Gia = c.Sach.GiaHienTai
                },
                SoLuong = c.SoLuong
            }).ToList();

            var thanhToan = new ThanhToanWindow(danhSachChiTiet, tongTien);
            var result = thanhToan.ShowDialog();
            if (result == true)
            {
                XoaSanPhamDaMua();
            }
        }

        private void XoaSanPhamDaMua()
        {
            var daMua = GioHangItems.Where(c => c.IsSelected).ToList();

            foreach (var item in daMua)
            {
                GioHangItems.Remove(item);
                var gioHangItem = GioHangHienTai.ChiTietGioHangs
                    .FirstOrDefault(c => c.Sach.SachId == item.Sach.SachId);

                if (gioHangItem != null)
                    GioHangHienTai.ChiTietGioHangs.Remove(gioHangItem);
            }

            TinhTongTien();
        }
    }

    // 🔹 ViewModel giỏ hàng
    public class ChiTietGioHangViewModel : INotifyPropertyChanged
    {
        private bool _isSelected;
        private int _soLuong;
        private SachViewModel _sach;

        public SachViewModel Sach
        {
            get => _sach;
            set
            {
                _sach = value;
                OnPropertyChanged(nameof(Sach));
                OnPropertyChanged(nameof(HinhAnhBitmap));
            }
        }

        public int SoLuong
        {
            get => _soLuong;
            set
            {
                if (_soLuong != value)
                {
                    _soLuong = value;
                    OnPropertyChanged(nameof(SoLuong));
                }
            }
        }

        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                if (_isSelected != value)
                {
                    _isSelected = value;
                    OnPropertyChanged(nameof(IsSelected));
                }
            }
        }
        public BitmapImage HinhAnhBitmap
        {
            get
            {
                string path = string.IsNullOrEmpty(Sach?.AnhSach)
                    ? "Views/KhachHang/no_image.png"
                    : Sach.AnhSach;

                string fullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, path);

                if (!File.Exists(fullPath))
                    fullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Views/KhachHang/no_image.png");

                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(fullPath, UriKind.Absolute);
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.EndInit();
                return bitmap;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }

    // 🔹 Model giỏ hàng
    public class GioHang
    {
        public ObservableCollection<ChiTietGioHang> ChiTietGioHangs { get; set; } = new ObservableCollection<ChiTietGioHang>();
    }
}
