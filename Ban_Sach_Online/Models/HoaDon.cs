using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ban_Sach_Online.Models
{
    public class HoaDon
    {
        public int HoaDonId { get; set; }
        public DateTime NgayLap { get; set; }
        public decimal TongTien { get; set; }
        public string DiaChiGiaoHang { get; set; }

        public string TrangThai { get; set; } = "Chờ xử lý";

        // Quan hệ
        public int KhachHangId { get; set; }
        public virtual KhachHang KhachHang { get; set; }

        public virtual ICollection<ChiTietHoaDon> ChiTietHoaDons { get; set; }
    }
}
