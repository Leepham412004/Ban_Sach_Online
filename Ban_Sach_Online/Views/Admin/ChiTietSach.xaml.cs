using Ban_Sach_Online.Models;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Media.Imaging;
using Ban_Sach_Online.Views.KhachHang;

namespace Ban_Sach_Online.Views.Admin
{
    public partial class ChiTietSach : Window
    {
        private Sach sach;
        public ChiTietSach(Sach sach)
        {
            InitializeComponent();
            this.sach = sach;
            HienThiThongTinSach();
        }

        private void HienThiThongTinSach()
        {
            txtTenSach.Text = sach.TenSach;
            txtTacGia.Text = sach.TacGia;
            txtNhaXB.Text = sach.NhaXB;
            txtNamXB.Text = sach.NamXB.ToString();
            txtTheLoai.Text = sach.TheLoai?.TenTheLoai;
            txtGia.Text = $"{sach.Gia:N0} đ";
            txtSoLuong.Text = sach.SoLuong.ToString();
            txtSoTrang.Text = sach.SoTrang.ToString();
            txtKichThuoc.Text = sach.KichThuoc;
            txtTrongLuong.Text = (sach.TrongLuong).ToString("0") + " gr";
            txtMoTa.Text = string.IsNullOrEmpty(sach.MoTa) ? "Chưa có mô tả" : sach.MoTa;

            // Load ảnh đầu tiên (nếu có)
            var anh = sach.AnhSachs.FirstOrDefault()?.Url;
            if (!string.IsNullOrEmpty(anh))
            {
                try
                {
                    imgSach.Source = new BitmapImage(new Uri($"pack://siteoforigin:,,,/{anh}"));
                }
                catch
                {
                    imgSach.Source = null;
                }
            }
        }

        private void btnDong_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
