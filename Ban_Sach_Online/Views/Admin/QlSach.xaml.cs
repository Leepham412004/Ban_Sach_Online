using Ban_Sach_Online.Data;
using Ban_Sach_Online.Models;
using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Data.Entity;

namespace Ban_Sach_Online.Views.Admin
{
    public partial class QlSach : Window
    {
        private readonly CSDL_Context _context = new CSDL_Context();

        public QlSach()
        {
            InitializeComponent();
            LoadSach();
        }

        // ✅ Hàm load toàn bộ danh sách sách (có ảnh)
        private void LoadSach()
        {
            var sachList = _context.Sachs
                .Include(s => s.AnhSachs)
                .Include(s => s.TheLoai)
                .ToList();
            foreach (var sach in sachList)
            {
                sach.SoLuongDaBan = _context.ChiTietHoaDons
                    .Where(ct => ct.SachId == sach.SachId)
                    .Sum(ct => (int?)ct.SoLuong) ?? 0;
            }
            lvSach.ItemsSource = sachList;
        }

        private void BtnTimKiem_Click(object sender, RoutedEventArgs e)
        {
            TimKiem();
        }

        // ✅ Nhấn Enter trong ô tìm kiếm
        private void txtTimKiem_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                TimKiem();
        }
        private void TimKiem()
        {
            string keyword = txtTimKiem.Text.Trim().ToLower();

            // Nếu không nhập gì -> load lại danh sách gốc
            if (string.IsNullOrEmpty(keyword))
            {
                LoadSach();
                return;
            }

            var result = _context.Sachs
                .Include(s => s.AnhSachs)
                .Include(s => s.TheLoai)
                .Where(s => s.TenSach.ToLower().Contains(keyword))
                .ToList();

            lvSach.ItemsSource = result;
        }

        // ✅ Sắp xếp danh sách (ảnh vẫn còn)
        private void CbSapXep_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbSapXep.SelectedItem is ComboBoxItem selected)
            {
                string tag = selected.Tag.ToString();
                var list = _context.Sachs
                    .Include(s => s.AnhSachs)
                    .Include(s => s.TheLoai)
                    .AsQueryable();

                switch (tag)
                {
                    case "id_asc": list = list.OrderBy(s => s.SachId); break;
                    case "id_desc": list = list.OrderByDescending(s => s.SachId); break;
                    case "date_asc": list = list.OrderBy(s => s.NgayThem); break;
                    case "date_desc": list = list.OrderByDescending(s => s.NgayThem); break;
                }
                lvSach.ItemsSource = list.ToList();
            }
        }

        // ✅ Double click mở chi tiết sách
        private void LvSach_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            dynamic selected = lvSach.SelectedItem;
            if (selected != null)
            {
                int sachId = selected.SachId;
                var sach = _context.Sachs
                    .Include(s => s.AnhSachs)
                    .Include(s => s.TheLoai)
                    .FirstOrDefault(s => s.SachId == sachId);

                if (sach != null)
                {
                    var detailWindow = new ChiTietSach(sach);
                    detailWindow.ShowDialog();
                }
            }
        }

        // ✅ Thêm sách
        private void BtnThemSach_Click(object sender, RoutedEventArgs e)
        {
            var themWindow = new ThemSuaSachWindow();
            if (themWindow.ShowDialog() == true)
                LoadSach();
        }

        // ✅ Sửa sách
        private void BtnSuaSach_Click(object sender, RoutedEventArgs e)
        {
            dynamic selected = lvSach.SelectedItem;
            if (selected != null)
            {
                int sachId = selected.SachId;
                var sach = _context.Sachs
                    .Include(s => s.AnhSachs)
                    .Include(s => s.TheLoai)
                    .FirstOrDefault(s => s.SachId == sachId);

                if (sach != null)
                {
                    var suaWindow = new ThemSuaSachWindow(sach.SachId);
                    if (suaWindow.ShowDialog() == true)
                        LoadSach();
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn sách để sửa.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        // ✅ Xóa sách (xóa cả ảnh vật lý)
        private void BtnXoaSach_Click(object sender, RoutedEventArgs e)
        {
            dynamic selected = lvSach.SelectedItem;
            if (selected != null)
            {
                int sachId = selected.SachId;
                var sach = _context.Sachs
                    .Include(s => s.AnhSachs)
                    .FirstOrDefault(s => s.SachId == sachId);

                if (sach != null)
                {
                    if (MessageBox.Show($"Bạn có chắc muốn xóa '{sach.TenSach}'?",
                        "Xác nhận", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                    {
                        // Xóa ảnh vật lý
                        foreach (var anh in sach.AnhSachs.ToList())
                        {
                            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, anh.Url);
                            try
                            {
                                if (File.Exists(path))
                                    File.Delete(path);
                            }
                            catch { /* bỏ qua lỗi file đang được sử dụng */ }
                            _context.AnhSachs.Remove(anh);
                        }

                        _context.Sachs.Remove(sach);
                        _context.SaveChanges();
                        LoadSach();
                    }
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn sách để xóa.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
        private void BtnNoiBat_Click(object sender, RoutedEventArgs e)
        {
            foreach (dynamic item in lvSach.Items)
            {
                int id = item.SachId;
                var sach = _context.Sachs.FirstOrDefault(x => x.SachId == id);
                if (sach != null)
                    sach.LaNoiBat = item.LaNoiBat; // cập nhật theo checkbox
            }

            _context.SaveChanges();
            MessageBox.Show("✅ Cập nhật danh sách sách nổi bật thành công!", "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);
            LoadSach();
        }
        // ✅ Cập nhật Flash Sale
        private void BtnFlashSale_Click(object sender, RoutedEventArgs e)
        {
            foreach (dynamic item in lvSach.Items)
            {
                int id = item.SachId;
                var sach = _context.Sachs.FirstOrDefault(x => x.SachId == id);
                if (sach != null)
                    sach.LaFlashSale = item.LaFlashSale; // lấy từ checkbox
            }

            _context.SaveChanges();
            MessageBox.Show("🔥 Cập nhật danh sách Flash Sale thành công!",
                            "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);
            LoadSach();
        }

        // ✅ Cập nhật Ngày Đôi
        private void BtnNgayDoi_Click(object sender, RoutedEventArgs e)
        {
            foreach (dynamic item in lvSach.Items)
            {
                int id = item.SachId;
                var sach = _context.Sachs.FirstOrDefault(x => x.SachId == id);
                if (sach != null)
                    sach.LaNgayDoi = item.LaNgayDoi; // lấy từ checkbox
            }

            _context.SaveChanges();
            MessageBox.Show("💕 Cập nhật danh sách Ngày Đôi thành công!",
                            "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);
            LoadSach();
        }
    }
}
