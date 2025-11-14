using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Input;
using System.Data.Entity;
using Ban_Sach_Online.Models;
using Ban_Sach_Online.Data;
using Ban_Sach_Online.Views.Admin;

namespace Ban_Sach_Online.Views.KhachHang
{
    public partial class TimKiem : Window
    {
        private int khachHangId;
        private ObservableCollection<Sach> AllBooks;
        private readonly CSDL_Context db;

        public TimKiem()
        {
            InitializeComponent();
            db = new CSDL_Context();
            LoadAllBooks();
            LoadTheLoai();
        }

        public TimKiem(string keyword) : this()
        {
            txtKeyword.Text = keyword;
            ApplyFilters();
        }

        public TimKiem(string keyword, List<Sach> ketQuaTruyenVao)
        {
            InitializeComponent();
            db = new CSDL_Context();

            if (ketQuaTruyenVao != null && ketQuaTruyenVao.Any())
                AllBooks = new ObservableCollection<Sach>(ketQuaTruyenVao);
            else
                AllBooks = new ObservableCollection<Sach>(
                    db.Sachs.Include(s => s.AnhSachs).Include(s => s.TheLoai).ToList()
                );

            txtKeyword.Text = keyword;
            ApplyFilters();
        }

        private void LoadAllBooks()
        {
            var allFromDb = db.Sachs
                .Include(s => s.AnhSachs)
                .Include(s => s.TheLoai)
                .ToList();

            AllBooks = new ObservableCollection<Sach>(allFromDb);
            LoadBooks(AllBooks);
        }

        private void LoadBooks(IEnumerable<Sach> books)
        {
            itemsKetQua.Items.Clear();

            if (books == null || !books.Any())
            {
                MessageBox.Show("Không có sách nào phù hợp.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                txtKeyword.Clear();
                LoadBooks(AllBooks);
                return;
            }
            foreach (var b in books)
            {
                // Tính tổng số đã bán
                b.SoLuongDaBan = db.ChiTietHoaDons
                                    .Where(ct => ct.SachId == b.SachId)
                                    .Sum(ct => (int?)ct.SoLuong) ?? 0;
            }

            foreach (var b in books)
            {
                var border = new Border
                {
                    Width = 220,
                    Height = 340,
                    Margin = new Thickness(10),
                    Background = System.Windows.Media.Brushes.White,
                    CornerRadius = new CornerRadius(10),
                    BorderBrush = System.Windows.Media.Brushes.LightGray,
                    BorderThickness = new Thickness(1),
                    Cursor = Cursors.Hand,
                    Tag = b // ✅ gán Tag để click nhận được sách
                };

                border.MouseLeftButtonUp += Border_MouseLeftButtonUp;

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
                infoStack.Children.Add(new TextBlock
                {
                    Text = b.TenSach,
                    FontWeight = FontWeights.Bold,
                    FontSize = 15,
                    TextWrapping = TextWrapping.Wrap
                });
                infoStack.Children.Add(new TextBlock
                {
                    Text = $"Tác giả: {b.TacGia}",
                    FontSize = 13,
                    Foreground = System.Windows.Media.Brushes.Gray
                });
                infoStack.Children.Add(new TextBlock
                {
                    Text = $"{b.Gia:N0} đ",
                    FontSize = 14,
                    Foreground = System.Windows.Media.Brushes.Red
                });

                if (b.TheLoai != null)
                {
                    infoStack.Children.Add(new TextBlock
                    {
                        Text = $"Thể loại: {b.TheLoai.TenTheLoai}",
                        FontSize = 12,
                        Foreground = System.Windows.Media.Brushes.DarkBlue
                    });
                }

                stack.Children.Add(infoStack);
                border.Child = stack;

                itemsKetQua.Items.Add(border);
            }
        }
        private BitmapImage LoadBitmap(string duongDanAnh)
        {
            try
            {
                var bitmap = new BitmapImage();
                bitmap.BeginInit();

                // Nếu admin lưu kiểu: "Images/Sach/sach1.jpg"
                if (!string.IsNullOrEmpty(duongDanAnh))
                {
                    // Bỏ prefix "pack://siteoforigin:,,," nếu có
                    duongDanAnh = duongDanAnh.Replace("pack://siteoforigin:,,,/", "");

                    string fullPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, duongDanAnh);

                    if (System.IO.File.Exists(fullPath))
                    {
                        bitmap.UriSource = new Uri(fullPath, UriKind.Absolute);
                    }
                    else
                    {
                        // fallback nếu file không tồn tại
                        bitmap.UriSource = new Uri("pack://application:,,,/Ban_Sach_Online;component/Views/KhachHang/no_image.png");
                    }
                }
                else
                {
                    // nếu đường dẫn rỗng
                    bitmap.UriSource = new Uri("pack://application:,,,/Ban_Sach_Online;component/Views/KhachHang/no_image.png");
                }

                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.EndInit();
                bitmap.Freeze();
                return bitmap;
            }
            catch
            {
                return new BitmapImage(new Uri("pack://application:,,,/Ban_Sach_Online;component/Views/KhachHang/no_image.png"));
            }
        }


        private void ApplyFilters()
        {
            if (AllBooks == null || AllBooks.Count == 0)
                return;

            string keyword = txtKeyword.Text.Trim().ToLower();
            IEnumerable<Sach> ketQua = AllBooks;

            if (!string.IsNullOrEmpty(keyword))
            {
                ketQua = ketQua.Where(s =>
                    (s.TenSach?.ToLower().Contains(keyword) ?? false) ||
                    (s.TacGia?.ToLower().Contains(keyword) ?? false) ||
                    (s.NhaXB?.ToLower().Contains(keyword) ?? false) ||
                    (s.TheLoai?.TenTheLoai?.ToLower().Contains(keyword) ?? false)
                );
            }

            if (!ketQua.Any())
            {
                MessageBox.Show("Không có sách nào phù hợp.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                itemsKetQua.Items.Clear();
                return;
            }

            LoadBooks(ketQua);
        }


        private void txtKeyword_TextChanged(object sender, TextChangedEventArgs e)
        {
            ApplyFilters();
        }

        private void txtKeyword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                ApplyFilters();
        }

        private void LoadTheLoai()
        {
            try
            {
                var theLoais = db.TheLoais.ToList();
                cboTheLoai.ItemsSource = theLoais;
                cboTheLoai.DisplayMemberPath = "TenTheLoai";   // Hiển thị tên
                cboTheLoai.SelectedValuePath = "TheLoaiId";    // Giá trị ID
                cboTheLoai.SelectedIndex = -1;                 // Mặc định không chọn
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải thể loại: " + ex.Message);
            }
        }

        private void Button_Loc_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string keyword = (txtKeyword?.Text ?? "").ToLower().Trim();
                decimal.TryParse(txtGiaMin?.Text, out decimal giaMin);
                decimal.TryParse(txtGiaMax?.Text, out decimal giaMax);

                // ✅ Lấy danh sách từ DB có đầy đủ quan hệ
                var query = db.Sachs
                    .Include(s => s.TheLoai)
                    .Include(s => s.AnhSachs)
                    .Include(s => s.DanhGias)
                    .AsQueryable();

                // 🔹 Lọc theo từ khóa
                if (!string.IsNullOrEmpty(keyword))
                {
                    query = query.Where(s =>
                        (s.TenSach != null && s.TenSach.ToLower().Contains(keyword)) ||
                        (s.TacGia != null && s.TacGia.ToLower().Contains(keyword)) ||
                        (s.NhaXB != null && s.NhaXB.ToLower().Contains(keyword)) ||
                        (s.TheLoai != null && s.TheLoai.TenTheLoai.ToLower().Contains(keyword))
                    );
                }

                // 🔹 Lọc theo thể loại
                if (cboTheLoai.SelectedItem is TheLoai selectedTheLoai)
                {
                    query = query.Where(s => s.TheLoaiId == selectedTheLoai.TheLoaiId);
                }

                // 🔹 Lọc theo giá
                if (giaMin > 0)
                    query = query.Where(s => s.Gia >= giaMin);
                if (giaMax > 0)
                    query = query.Where(s => s.Gia <= giaMax);

                // Lấy danh sách tạm để xử lý thêm
                var resultList = query.ToList();

                // 🔹 Lọc theo số sao đánh giá (client-side)
                if (cboDanhGia.SelectedItem is ComboBoxItem selectedRating)
                {
                    if (int.TryParse(selectedRating.Content.ToString().Split(' ')[0], out int minStars))
                    {
                        resultList = resultList
                            .Where(s => s.DanhGias != null && s.DanhGias.Any() &&
                                        Math.Round(s.DanhGias.Average(d => d.SoSao)) >= minStars)
                            .ToList();
                    }
                }

                // ✅ Đảm bảo ảnh luôn hợp lệ, tránh crash
                foreach (var s in resultList)
                {
                    if (s.AnhSachs == null || !s.AnhSachs.Any())
                    {
                        // fallback: thêm ảnh mặc định nếu sách chưa có ảnh
                        s.AnhSachs = new List<AnhSach>
                {
                    new AnhSach { Url = "Views/KhachHang/no_image.png" }
                };
                    }
                }

                // 🔹 Hiển thị kết quả
                if (resultList.Any())
                {
                    LoadBooks(resultList);
                }
                else
                {
                    MessageBox.Show("Không tìm thấy sách phù hợp với bộ lọc.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                    itemsKetQua.Items.Clear();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi thực hiện lọc: " + ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        // ✅ Xử lý khi click vào 1 sách bất kỳ
        private void Border_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (sender is Border border && border.Tag is Sach sach)
            {
                // Mở trang chi tiết sách cho khách hàng
                ChiTietSach_KH chiTiet = new ChiTietSach_KH(sach.SachId, khachHangId);
                chiTiet.ShowDialog();
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

        private void cboTheLoai_DropDownOpened(object sender, EventArgs e)
        {
            try
            {
                // Lấy dữ liệu từ QlDanhMuc (admin)
                var danhMucTheLoais = db.TheLoais.ToList(); // hoặc db.QlDanhMuc nếu table là QlDanhMuc
                cboTheLoai.ItemsSource = danhMucTheLoais;
                cboTheLoai.DisplayMemberPath = "TenTheLoai";
                cboTheLoai.SelectedValuePath = "TheLoaiId";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi load thể loại: " + ex.Message);
            }
        }
    }
}
