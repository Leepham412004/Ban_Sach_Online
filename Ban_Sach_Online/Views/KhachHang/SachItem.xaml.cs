using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Ban_Sach_Online.Models;

namespace Ban_Sach_Online.Views.KhachHang
{
    public partial class SachItem : UserControl
    {
        private int khachHangId;
        public SachItem()
        {
            InitializeComponent();
        }

        private void ThemVaoGio_Click(object sender, RoutedEventArgs e)
        {
            var sachVM = DataContext as SachViewModel;
            if (sachVM == null) return;

            if (!sachVM.IsConHang)
            {
                MessageBox.Show("Sách này đã hết hàng, không thể thêm vào giỏ.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var sach = new Sach
            {
                SachId = sachVM.SachId,
                TenSach = sachVM.TenSach,
                TacGia = sachVM.TacGia,
                Gia = sachVM.GiaSauGiam > 0 ? sachVM.GiaSauGiam : sachVM.GiaGoc,
                MoTa = sachVM.MoTa,
                AnhSachs = new List<AnhSach> { new AnhSach { Url = sachVM.AnhSach } }
            };

            CartWindow.ThemSachVaoGio(sach);

            // Giảm số lượng sách sau khi thêm vào giỏ
            sachVM.SoLuongCon--;

            MessageBox.Show($"{sach.TenSach} đã được thêm vào giỏ hàng.", "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void MuaNgay_Click(object sender, RoutedEventArgs e)
        {
            var sachVM = DataContext as SachViewModel;
            if (sachVM == null) return;

            if (!sachVM.IsConHang)
            {
                MessageBox.Show("Sách này đã hết hàng, không thể mua ngay.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var chiTiet = new ChiTietSach_KH(sachVM.SachId, khachHangId);
            chiTiet.ShowDialog();
        }
    }
}
