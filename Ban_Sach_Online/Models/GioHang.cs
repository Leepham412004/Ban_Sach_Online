using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Ban_Sach_Online.Models
{
    public class GioHang
    {
        [Key, ForeignKey("KhachHang")]
        public int GioHangId { get; set; }
        public virtual KhachHang KhachHang { get; set; }

        // ✅ Dùng ObservableCollection để hỗ trợ CollectionChanged
        public virtual ObservableCollection<ChiTietGioHang> ChiTietGioHangs { get; set; } = new ObservableCollection<ChiTietGioHang>();

        // Các thuộc tính hỗ trợ UI (không lưu DB)
        [NotMapped]
        public decimal TongTien => ChiTietGioHangs?.Sum(c => c.TongTien) ?? 0;

        [NotMapped]
        public string TongTienHienThi => $"{TongTien:N0} đ";

        [NotMapped]
        public int TongSoLuong => ChiTietGioHangs?.Sum(c => c.SoLuong) ?? 0;
    }
}
