using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;

namespace Ban_Sach_Online.Models
{
    public class ChiTietGioHang : INotifyPropertyChanged
    {
        public int ChiTietGioHangId { get; set; }

        public int GioHangId { get; set; }
        public virtual GioHang GioHang { get; set; }

        public int SachId { get; set; }
        public virtual Sach Sach { get; set; }

        private int soLuong = 1;
        public int SoLuong
        {
            get => soLuong;
            set
            {
                if (soLuong != value)
                {
                    soLuong = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(TongTien));
                }
            }
        }

        private bool isSelected = true;
        [NotMapped]
        public bool IsSelected
        {
            get => isSelected;
            set
            {
                if (isSelected != value)
                {
                    isSelected = value;
                    OnPropertyChanged();
                }
            }
        }

        [NotMapped]
        public decimal TongTien => Sach != null ? Sach.Gia * SoLuong : 0;

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
