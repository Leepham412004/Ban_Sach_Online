using System;
using System.ComponentModel;
using System.Windows.Media.Imaging;

namespace Ban_Sach_Online.Views.KhachHang
{
    public class SachViewModel : INotifyPropertyChanged
    {
        public int SachId { get; set; }
        public string TenSach { get; set; }
        public string TacGia { get; set; }
        public string AnhSach { get; set; }
        public decimal GiaGoc { get; set; }
        public decimal GiaSauGiam { get; set; }
        public int PhanTramGiam { get; set; }
        public string MoTa { get; set; }
        public int SoLuongDaBan { get; set; }

        private int soLuongCon;
        public int SoLuongCon
        {
            get => soLuongCon;
            set
            {
                if (soLuongCon != value)
                {
                    soLuongCon = value;
                    OnPropertyChanged(nameof(SoLuongCon));
                    OnPropertyChanged(nameof(IsConHang));
                    OnPropertyChanged(nameof(TinhTrang));
                    OnPropertyChanged(nameof(MauTinhTrang));
                }
            }
        }

        public bool IsConHang => SoLuongCon > 0;

        public decimal GiaHienTai => GiaSauGiam > 0 ? GiaSauGiam : GiaGoc;

        public string TinhTrang => SoLuongCon > 0 ? $"{SoLuongCon} còn" : "Hết hàng";

        public string MauTinhTrang => SoLuongCon > 0 ? "Green" : "Red";
        private BitmapImage hinhAnhBitmap;
        public BitmapImage HinhAnhBitmap
        {
            get => hinhAnhBitmap;
            set
            {
                if (hinhAnhBitmap != value)
                {
                    hinhAnhBitmap = value;
                    OnPropertyChanged(nameof(HinhAnhBitmap));
                }
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
