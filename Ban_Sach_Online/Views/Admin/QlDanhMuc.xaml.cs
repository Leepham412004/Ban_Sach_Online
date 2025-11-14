using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Ban_Sach_Online.Data;
using Ban_Sach_Online.Models;
using System.Collections.ObjectModel;
using System.Windows.Media;

namespace Ban_Sach_Online.Views.Admin
{
    public partial class QlDanhMuc : Window
    {
        private CSDL_Context db;
        private ObservableCollection<TheLoai> danhSachTheLoai;

        public QlDanhMuc()
        {
            InitializeComponent();

            db = new CSDL_Context();
            danhSachTheLoai = new ObservableCollection<TheLoai>();

            Loaded += (s, e) => LoadData(); // Gọi LoadData sau khi UI load xong
        }

        // Nạp toàn bộ dữ liệu thể loại
        private void LoadData()
        {
            try
            {
                if (db == null) db = new CSDL_Context();

                var list = db.TheLoais.OrderBy(t => t.TheLoaiId).ToList();
                danhSachTheLoai = new ObservableCollection<TheLoai>(list);

                if (dgTheLoai != null)
                    dgTheLoai.ItemsSource = danhSachTheLoai;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu: " + ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Thêm thể loại
        private void Button_Them_Click(object sender, RoutedEventArgs e)
        {
            string ten = txtTenTheLoai.Text?.Trim();
            if (string.IsNullOrWhiteSpace(ten))
            {
                MessageBox.Show("Vui lòng nhập tên thể loại.", "Thông báo");
                return;
            }

            if (db.TheLoais.Any(t => t.TenTheLoai.ToLower() == ten.ToLower()))
            {
                MessageBox.Show("Thể loại này đã tồn tại.", "Thông báo");
                return;
            }

            var newTheLoai = new TheLoai { TenTheLoai = ten };
            db.TheLoais.Add(newTheLoai);
            db.SaveChanges();

            danhSachTheLoai.Add(newTheLoai);
            txtTenTheLoai.Clear();
        }

        // Nút tìm kiếm
        private void BtnTimKiem_Click(object sender, RoutedEventArgs e)
        {
            DoTimKiem();
        }

        // Hàm tìm kiếm chung
        private void DoTimKiem()
        {
            string keyword = txtTimKiem.Text?.Trim();

            if (string.IsNullOrEmpty(keyword) || keyword.Equals("Nhập tên thể loại...", StringComparison.InvariantCultureIgnoreCase))
            {
                LoadData();
                return;
            }

            var result = db.TheLoais
                .Where(t => t.TenTheLoai.ToLower().Contains(keyword.ToLower()))
                .OrderBy(t => t.TheLoaiId)
                .ToList();

            danhSachTheLoai = new ObservableCollection<TheLoai>(result);
            dgTheLoai.ItemsSource = danhSachTheLoai;

            if (result.Count == 0)
                MessageBox.Show("Không tìm thấy thể loại nào phù hợp.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        // Tự động hiển thị lại toàn bộ khi xóa chữ
        private void txtTimKiem_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (txtTimKiem.Text == "" || txtTimKiem.Text == "Nhập tên thể loại...")
            {
                LoadData();
            }
        }

        // Placeholder behavior
        private void txtTimKiem_GotFocus(object sender, RoutedEventArgs e)
        {
            if (txtTimKiem.Text == "Nhập tên thể loại...")
            {
                txtTimKiem.Text = "";
                txtTimKiem.Foreground = Brushes.Black;
            }
        }

        private void txtTimKiem_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTimKiem.Text))
            {
                txtTimKiem.Text = "Nhập tên thể loại...";
                txtTimKiem.Foreground = Brushes.Gray;
            }
        }

        private void txtTimKiem_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                DoTimKiem();
            }
        }

        private void dgTheLoai_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (dgTheLoai.SelectedItem is TheLoai selected)
            {
                txtTenTheLoai.Text = selected.TenTheLoai;
            }
        }

        // Sửa thể loại
        private void Button_Sua_Click(object sender, RoutedEventArgs e)
        {
            if (dgTheLoai.SelectedItem is TheLoai selected)
            {
                string tenMoi = txtTenTheLoai.Text?.Trim();
                if (string.IsNullOrWhiteSpace(tenMoi))
                {
                    MessageBox.Show("Vui lòng nhập tên thể loại.", "Thông báo");
                    return;
                }

                if (db.TheLoais.Any(t => t.TheLoaiId != selected.TheLoaiId && t.TenTheLoai.ToLower() == tenMoi.ToLower()))
                {
                    MessageBox.Show("Tên thể loại đã tồn tại.", "Thông báo");
                    return;
                }

                var entity = db.TheLoais.Find(selected.TheLoaiId);
                if (entity != null)
                {
                    entity.TenTheLoai = tenMoi;
                    db.SaveChanges();

                    selected.TenTheLoai = tenMoi;
                    dgTheLoai.Items.Refresh();
                    txtTenTheLoai.Clear();
                }
                else
                {
                    LoadData();
                    MessageBox.Show("Không tìm thấy thể loại trong cơ sở dữ liệu. Danh sách đã được làm mới.", "Thông báo");
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn thể loại để sửa.", "Thông báo");
            }
        }

        // Xóa thể loại
        private void Button_Xoa_Click(object sender, RoutedEventArgs e)
        {
            if (dgTheLoai.SelectedItem is TheLoai selected)
            {
                var confirm = MessageBox.Show($"Bạn có muốn xóa thể loại '{selected.TenTheLoai}'?", "Xác nhận", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (confirm != MessageBoxResult.Yes) return;

                var entity = db.TheLoais.Find(selected.TheLoaiId);
                if (entity != null)
                {
                    db.TheLoais.Remove(entity);
                    db.SaveChanges();

                    danhSachTheLoai.Remove(selected);
                }
                else
                {
                    LoadData();
                    MessageBox.Show("Không tìm thấy thể loại trong cơ sở dữ liệu. Danh sách đã được làm mới.", "Thông báo");
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn thể loại để xóa.", "Thông báo");
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            db?.Dispose();
        }
    }
}
