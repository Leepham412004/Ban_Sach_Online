using Ban_Sach_Online.Data;
using Ban_Sach_Online.Models;
using Microsoft.Win32;
using System;
using System.IO;
using System.Linq;
using System.Windows;

namespace Ban_Sach_Online.Views.Admin
{
    public partial class ThemSuaSachWindow : Window
    {
        private readonly CSDL_Context _context = new CSDL_Context();
        private Sach _sachSua; // null => thêm mới

        // ✅ Constructor sửa: truyền vào ID
        public ThemSuaSachWindow(int? sachId = null)
        {
            InitializeComponent();
            LoadTheLoai();

            if (sachId.HasValue)
            {
                // ✅ Load lại sách theo ID
                _sachSua = _context.Sachs
                                   .Include("TheLoai")
                                   .Include("AnhSachs")
                                   .FirstOrDefault(s => s.SachId == sachId.Value);

                if (_sachSua != null)
                    LoadDataForEdit();
            }
        }

        private void LoadTheLoai()
        {
            var dsTheLoai = _context.TheLoais.ToList();
            cbTheLoai.ItemsSource = dsTheLoai;
            cbTheLoai.DisplayMemberPath = "TenTheLoai";
            cbTheLoai.SelectedValuePath = "TheLoaiId";
        }

        private void LoadDataForEdit()
        {
            txtTenSach.Text = _sachSua.TenSach;
            txtTacGia.Text = _sachSua.TacGia;
            txtNhaXB.Text = _sachSua.NhaXB;
            txtNamXB.Text = _sachSua.NamXB.ToString();
            txtGia.Text = _sachSua.Gia.ToString("0");
            txtSoLuong.Text = _sachSua.SoLuong.ToString();
            txtMoTa.Text = _sachSua.MoTa;
            txtHinhAnh.Text = _sachSua.AnhSachs.FirstOrDefault()?.Url ?? "";

            if (_sachSua.TheLoai != null)
                cbTheLoai.SelectedValue = _sachSua.TheLoai.TheLoaiId;
        }

        private void BtnChonAnh_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new OpenFileDialog
            {
                Filter = "Ảnh (*.jpg;*.jpeg;*.png;*.bmp)|*.jpg;*.jpeg;*.png;*.bmp"
            };

            if (dlg.ShowDialog() == true)
            {
                try
                {
                    string folder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Images", "Sach");
                    if (!Directory.Exists(folder))
                        Directory.CreateDirectory(folder);

                    string fileName = Path.GetFileName(dlg.FileName);
                    string destPath = Path.Combine(folder, fileName);

                    // Nếu file trùng tên, thêm GUID
                    if (File.Exists(destPath))
                    {
                        destPath = Path.Combine(folder, Guid.NewGuid().ToString() + "_" + fileName);
                    }

                    File.Copy(dlg.FileName, destPath, false);
                    txtHinhAnh.Text = $"Images/Sach/{Path.GetFileName(destPath)}";
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Không thể sao chép ảnh: " + ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void BtnLuu_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtTenSach.Text) || cbTheLoai.SelectedItem == null)
                {
                    MessageBox.Show("Vui lòng nhập tên sách và chọn thể loại.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                var theLoaiChon = cbTheLoai.SelectedItem as TheLoai;
                int.TryParse(txtNamXB.Text, out int namXB);
                int.TryParse(txtSoLuong.Text, out int soLuong);
                decimal.TryParse(txtGia.Text, out decimal gia);
                int.TryParse(txtSoTrang.Text, out int soTrang);
                decimal.TryParse(txtTrongLuong.Text, out decimal trongLuong);
                string kichThuoc = txtKichThuoc.Text.Trim();

                if (_sachSua == null)
                {
                    var sachMoi = new Sach
                    {
                        TenSach = txtTenSach.Text.Trim(),
                        TacGia = txtTacGia.Text.Trim(),
                        NhaXB = txtNhaXB.Text.Trim(),
                        NamXB = namXB,
                        Gia = gia,
                        SoLuong = soLuong,
                        SoTrang = soTrang,
                        KichThuoc = kichThuoc,
                        TrongLuong = trongLuong,
                        MoTa = txtMoTa.Text.Trim(),
                        NgayThem = DateTime.Now,
                        TheLoaiId = theLoaiChon.TheLoaiId
                    };

                    if (!string.IsNullOrEmpty(txtHinhAnh.Text))
                        sachMoi.AnhSachs.Add(new AnhSach { Url = txtHinhAnh.Text });

                    _context.Sachs.Add(sachMoi);
                }
                else
                {
                    _sachSua.TenSach = txtTenSach.Text.Trim();
                    _sachSua.TacGia = txtTacGia.Text.Trim();
                    _sachSua.NhaXB = txtNhaXB.Text.Trim();
                    _sachSua.NamXB = namXB;
                    _sachSua.Gia = gia;
                    _sachSua.SoLuong = soLuong;
                    _sachSua.SoTrang = soTrang;
                    _sachSua.KichThuoc = kichThuoc;
                    _sachSua.TrongLuong = trongLuong;
                    _sachSua.MoTa = txtMoTa.Text.Trim();
                    _sachSua.TheLoaiId = theLoaiChon.TheLoaiId;

                    if (!string.IsNullOrEmpty(txtHinhAnh.Text))
                    {
                        var anh = _sachSua.AnhSachs.FirstOrDefault();
                        if (anh != null)
                            anh.Url = txtHinhAnh.Text;
                        else
                            _sachSua.AnhSachs.Add(new AnhSach { Url = txtHinhAnh.Text });
                    }
                }

                _context.SaveChanges();
                MessageBox.Show("Lưu thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);

                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi lưu: " + ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void BtnHuy_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
