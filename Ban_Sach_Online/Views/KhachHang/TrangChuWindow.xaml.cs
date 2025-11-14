using System;
using System.IO;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using Ban_Sach_Online.Models;
using Ban_Sach_Online.Data;
using System.Data.Entity;
using System.Windows.Input;
using System.Windows.Media;

namespace Ban_Sach_Online.Views.KhachHang
{
    public partial class TrangChuWindow : Window
    {
        private int khachHangId;
        private DispatcherTimer bannerTimer;
        private DispatcherTimer flashSaleTimer;
        private TimeSpan remainingTime;
        private string[] bannerImages;
        private int currentIndex = 0;

        // 🧍 Lưu thông tin khách hàng đang đăng nhập
        private Ban_Sach_Online.Models.KhachHang _khachHang;
        private readonly CSDL_Context _db = new CSDL_Context();

        // Danh sách sách
        public ObservableCollection<Sach> FlashSaleBooks { get; set; }
        public ObservableCollection<Sach> NoiBatBooks { get; set; }
        public ObservableCollection<Sach> BanChayBooks { get; set; }
        private ObservableCollection<Sach> AllBooks { get; set; }

        // 👉 Constructor mặc định (phải có để XAML hoạt động)
        public TrangChuWindow()
        {
            InitializeComponent();
            KhoiTaoBanner();
            KhoiTaoFlashSale();
        }

        // 👉 Constructor nhận thông tin khách hàng đăng nhập
        public TrangChuWindow(Ban_Sach_Online.Models.KhachHang khachHang) : this()
        {
            _khachHang = khachHang;
            MessageBox.Show($"Xin chào, {_khachHang.HoTen}!", "Chào mừng", MessageBoxButton.OK, MessageBoxImage.Information);
            LoadDanhMuc();
        }

        // 🔹 Khởi tạo banner
        private void KhoiTaoBanner()
        {
            bannerImages = new string[]
            {
                "Views/KhachHang/qc1.png",
                "Views/KhachHang/qc2.png",
                "Views/KhachHang/qc3.png",
            };

            if (bannerImages.Length > 0)
                imgBanner.Source = LoadBitmap(bannerImages[0]);

            bannerTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(2)
            };
            bannerTimer.Tick += BannerTimer_Tick;
            Loaded += (s, e) => bannerTimer.Start();
        }

        // 🔹 Khởi tạo flash sale
        private void KhoiTaoFlashSale()
        {
            remainingTime = TimeSpan.FromHours(1);

            flashSaleTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };
            flashSaleTimer.Tick += FlashSaleTimer_Tick;
            flashSaleTimer.Start();

            FlashSaleBooks = new ObservableCollection<Sach>(
              _db.Sachs
                 .Include(s => s.AnhSachs)
                 .Where(s => s.LaFlashSale)
                 .Take(10)
                 .ToList()
          );
            NoiBatBooks = new ObservableCollection<Sach>(
                _db.Sachs
                   .Include(s => s.AnhSachs)
                   .Where(s => s.LaNoiBat)
                   .Take(10)
                   .ToList()
            ); LoadBooks(itemsNoiBat, NoiBatBooks);

            BanChayBooks = new ObservableCollection<Sach>
            {
                new Sach { TenSach = "Sách Bán Chạy 1", TacGia = "Tác giả F", Gia = 100000,
                    AnhSachs = new[] { new AnhSach { Url = "Views/KhachHang/sach6.jpg" } } },
                new Sach { TenSach = "Sách Bán Chạy 2", TacGia = "Tác giả G", Gia = 120000,
                    AnhSachs = new[] { new AnhSach { Url = "Views/KhachHang/sach7.jpg" } } }
            };

            AllBooks = new ObservableCollection<Sach>(
                FlashSaleBooks.Concat(NoiBatBooks).Concat(BanChayBooks)
            );

            LoadBooks(itemsFlashSale, FlashSaleBooks);
            LoadBooks(itemsNoiBat, NoiBatBooks);
            LoadBooks(itemsBanChay, BanChayBooks);
        }

        // 🕐 Flash sale đếm ngược
        private void FlashSaleTimer_Tick(object sender, EventArgs e)
        {
            if (remainingTime.TotalSeconds > 0)
            {
                remainingTime = remainingTime.Subtract(TimeSpan.FromSeconds(1));
                txtFlashSaleTime.Text = $"⏰ Còn lại: {remainingTime:hh\\:mm\\:ss}";
            }
            else
            {
                txtFlashSaleTime.Text = "🎉 Hết thời gian Flash Sale!";
                flashSaleTimer.Stop();
            }
        }

        // 🖼 Banner tự động chuyển
        private void BannerTimer_Tick(object sender, EventArgs e)
        {
            if (bannerImages.Length == 0) return;
            currentIndex = (currentIndex + 1) % bannerImages.Length;
            imgBanner.Source = LoadBitmap(bannerImages[currentIndex]);
        }
        private BitmapImage LoadBitmap(string relativePath)
        {
            try
            {
                string fullPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, relativePath);
                if (!File.Exists(fullPath))
                    return null;

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


        private void LoadBooks(ItemsControl itemsControl, ObservableCollection<Sach> books)
        {
            itemsControl.Items.Clear();

            foreach (var b in books)
            {
                var border = new Border
                {
                    Width = 220,
                    Height = 360,
                    Margin = new Thickness(10),
                    Background = System.Windows.Media.Brushes.White,
                    CornerRadius = new CornerRadius(10),
                    BorderBrush = System.Windows.Media.Brushes.LightGray,
                    BorderThickness = new Thickness(1),
                    Cursor = Cursors.Hand,
                    Tag = b
                };
                border.MouseLeftButtonUp += (s, e) =>
                {
                    var sach = (s as Border)?.Tag as Sach;
                    if (sach != null)
                    {
                        // Mở cửa sổ chi tiết sách
                        var chiTietWindow = new ChiTietSach_KH(sach.SachId, khachHangId);
                        chiTietWindow.ShowDialog();
                    }
                };
                var stack = new StackPanel();
                string duongDanAnh = (b.AnhSachs != null && b.AnhSachs.Any())
                    ? b.AnhSachs.First().Url
                    : "Views/KhachHang/no_image.png";

                var img = new Image
                {
                    Source = LoadBitmap(duongDanAnh),
                    Height = 180,
                    Stretch = System.Windows.Media.Stretch.UniformToFill,
                    Margin = new Thickness(0, 0, 0, 5)
                };
                stack.Children.Add(img);

                var infoStack = new StackPanel { Margin = new Thickness(10) };
                var tenSachText = new TextBlock
                {
                    Text = b.TenSach,
                    FontWeight = FontWeights.Bold,
                    FontSize = 15,
                    Foreground = Brushes.Black,
                    TextTrimming = TextTrimming.CharacterEllipsis, // Cắt bớt nếu dài
                    TextWrapping = TextWrapping.NoWrap,             // ❌ Không xuống dòng
                    ToolTip = b.TenSach,                            // ✅ Hiện tooltip khi hover
                    Width = 180
                };
                infoStack.Children.Add(tenSachText);

                infoStack.Children.Add(new TextBlock
                {
                    Text = $"Tác giả: {b.TacGia}",
                    FontSize = 13,
                    Foreground = System.Windows.Media.Brushes.Gray,
                    Margin = new Thickness(0, 2, 0, 0)
                });
                infoStack.Children.Add(new TextBlock
                {
                    Text = $"{b.Gia:N0} đ",
                    FontSize = 14,
                    Foreground = System.Windows.Media.Brushes.Red,
                    Margin = new Thickness(0, 5, 0, 10)
                });

                int soLuongDaBan = _db.ChiTietHoaDons
                .Where(ct => ct.SachId == b.SachId)
                .Sum(ct => (int?)ct.SoLuong) ?? 0;
                infoStack.Children.Add(new TextBlock
                {
                    Text = $"Đã bán: {soLuongDaBan} cuốn",
                    FontSize = 12,
                    Foreground = System.Windows.Media.Brushes.Gray,
                    Margin = new Thickness(0, 5, 0, 5)
                });
                var btn = new Button
                {
                    Content = "🛒 Thêm vào giỏ",
                    Height = 32,
                    Width =180,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    Style = (Style)FindResource("RoundedButton"),
                    ToolTip = "Thêm sách vào giỏ hàng"
                };
                btn.Click += (s, e) =>
                {
                    CartWindow.ThemSachVaoGio(b);
                    MessageBox.Show($"Đã thêm \"{b.TenSach}\" vào giỏ hàng!", "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);
                };
                infoStack.Children.Add(btn);


                stack.Children.Add(infoStack);
                border.Child = stack;
                itemsControl.Items.Add(border);
            }
        }

        // 🔍 Tìm kiếm realtime
        private void txtTimKiem_TextChanged(object sender, TextChangedEventArgs e)
        {
            string keyword = txtTimKiem.Text.ToLower().Trim();

            if (keyword.Length < 3)
            {
                // Nếu chưa gõ đủ 3 ký tự thì hiển thị lại toàn bộ
                LoadBooks(itemsFlashSale, FlashSaleBooks);
                LoadBooks(itemsNoiBat, NoiBatBooks);
                LoadBooks(itemsBanChay, BanChayBooks);
                return;
            }

            var ketQua = AllBooks
                .Where(s => s.TenSach.ToLower().Contains(keyword) ||
                            s.TacGia.ToLower().Contains(keyword))
                .ToList();

            itemsFlashSale.Items.Clear();
            LoadBooks(itemsFlashSale, new ObservableCollection<Sach>(ketQua));
        }


        // Các nút khác
        private void Button_Click_TimKiem(object sender, RoutedEventArgs e)
        {
            string keyword = txtTimKiem.Text.Trim();
            if (string.IsNullOrEmpty(keyword))
            {
                MessageBox.Show("Vui lòng nhập từ khóa tìm kiếm.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Lọc sách theo từ khóa
            var ketQua = AllBooks
                .Where(s => s.TenSach.ToLower().Contains(keyword.ToLower()) ||
                            s.TacGia.ToLower().Contains(keyword.ToLower()))
                .ToList();
            if (ketQua == null || ketQua.Count == 0)
            {
                MessageBox.Show("Không có sách nào phù hợp!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                txtTimKiem.Text = "";
                return;
            }

            // Mở cửa sổ TimKiem
            TimKiem timKiemWindow = new TimKiem(keyword,ketQua);
            timKiemWindow.Owner = this;
            timKiemWindow.ShowDialog();

        }


        private void btnDanhMuc_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            popupDanhMuc.IsOpen = true;
        }

        private void btnDanhMuc_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (!popupDanhMuc.IsMouseOver && !btnDanhMuc.IsMouseOver)
                popupDanhMuc.IsOpen = false;
        }
        private void DanhMuc_Click(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (sender is Border border && border.Child is StackPanel sp &&
                sp.Children.OfType<TextBlock>().LastOrDefault() is TextBlock tb)
            {
                string theLoai = tb.Text;
                var ketQua = AllBooks.Where(s => s.TheLoai?.TenTheLoai == theLoai).ToList();
                LoadBooks(itemsFlashSale, new ObservableCollection<Sach>(ketQua));
                popupDanhMuc.IsOpen = false;
            }
        }
        private void LoadDanhMuc()
        {
            popupDanhMuc.Child = null;
            var danhMucs = _db.TheLoais
                              .OrderBy(dm => dm.TenTheLoai)
                              .ToList();
            var stack = new StackPanel();

            foreach (var dm in danhMucs)
            {
                var border = new Border
                {
                    Margin = new Thickness(5),
                    Padding = new Thickness(5),
                    CornerRadius = new CornerRadius(8),
                    Background = System.Windows.Media.Brushes.White,
                    Cursor = System.Windows.Input.Cursors.Hand,
                    RenderTransformOrigin = new Point(0.5, 0.5),
                    RenderTransform = new ScaleTransform(1, 1)
                };
                border.MouseDown += DanhMuc_Click; // sự kiện click

                var sp = new StackPanel { Orientation = Orientation.Horizontal, VerticalAlignment = VerticalAlignment.Center };

                // Icon chung
                var icon = new TextBlock
                {
                    Text = "📖",
                    FontSize = 16,
                    Margin = new Thickness(0, 0, 10, 0)
                };

                var txt = new TextBlock
                {
                    Text = dm.TenTheLoai,
                    FontSize = 14,
                    FontWeight = FontWeights.SemiBold,
                    Foreground = (System.Windows.Media.Brush)new System.Windows.Media.BrushConverter().ConvertFromString("#3B82F6")
                };

                sp.Children.Add(icon);
                sp.Children.Add(txt);
                border.Child = sp;

                // Hiệu ứng hover: đổi màu + phóng to
                var style = new Style(typeof(Border));
                var trigger = new Trigger { Property = Border.IsMouseOverProperty, Value = true };
                trigger.Setters.Add(new Setter(Border.BackgroundProperty, (System.Windows.Media.Brush)new System.Windows.Media.BrushConverter().ConvertFromString("#BFDBFE")));
                trigger.Setters.Add(new Setter(Border.RenderTransformProperty, new ScaleTransform(1.05, 1.05)));
                style.Triggers.Add(trigger);
                border.Style = style;

                stack.Children.Add(border);
            }

            var borderRoot = new Border
            {
                Background = System.Windows.Media.Brushes.White,
                BorderBrush = System.Windows.Media.Brushes.LightGray,
                BorderThickness = new Thickness(1),
                CornerRadius = new CornerRadius(10),
                Padding = new Thickness(10),
                Child = stack
            };

            popupDanhMuc.Child = borderRoot;
        }

        private void Button_Click_GioHang(object sender, RoutedEventArgs e)
        {
            var gioHang = new Ban_Sach_Online.Models.GioHang
            {
                GioHangId = _khachHang?.KhachHangId ?? 0
            };
            var gioHangForm = new CartWindow();
            gioHangForm.ShowDialog();
        }


        private void Button_Click_User(object sender, RoutedEventArgs e)
        {
            var userWindow = new TaiKhoan(_khachHang);
            userWindow.Owner = this;
            userWindow.ShowDialog();
        }

        private void Button_Click_SuKien(object sender, RoutedEventArgs e)
        {
            var suKienWindow = new SuKienWindow(khachHangId);
            suKienWindow.ShowDialog();
        }

        private void Button_Click_NgoaiVan(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Xem sách ngoại văn!");
        }

        private void Button_Click_McBooks(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Thương hiệu MCBooks!");
        }

        private void Button_Click_MaGiamGia(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Xem mã giảm giá hấp dẫn!");
        }

        private void Button_Click_SpMoi(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Xem sản phẩm mới nhất!");
        }

        private void Button_Click_sale(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Sách đang giảm giá đặc biệt!");
        }

        private void Button_Click_Manga(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Xem truyện tranh manga!");
        }

        private void txtTimKiem_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                string keyword = txtTimKiem.Text.ToLower().Trim();
                if (string.IsNullOrEmpty(keyword)) return;

                // Lấy kết quả từ AllBooks
                var ketQua = AllBooks
                    .Where(s => s.TenSach.ToLower().Contains(keyword) ||
                                s.TacGia.ToLower().Contains(keyword))
                    .ToList();

                // Mở TimKiem.xaml, truyền danh sách sách đã lọc
                var timKiemWindow = new TimKiem(keyword,ketQua);
                timKiemWindow.Owner = this;
                timKiemWindow.ShowDialog();
            }
        }
        private void Sach_Click(object sender, MouseButtonEventArgs e)
        {
            if (sender is Border border && border.Tag is Sach sach)
            {
                // Mở cửa sổ chi tiết sách khách hàng
                ChiTietSach_KH chiTiet = new ChiTietSach_KH(sach.SachId, khachHangId);
                chiTiet.ShowDialog();
            }
        }

    }
}
