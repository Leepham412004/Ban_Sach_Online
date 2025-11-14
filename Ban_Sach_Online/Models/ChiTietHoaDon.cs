using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ban_Sach_Online.Models
{
    public class ChiTietHoaDon
    {
        public int ChiTietHoaDonId { get; set; }
        public int SoLuong { get; set; }
        public decimal DonGia { get; set; }

        // Quan hệ
        public int HoaDonId { get; set; }
        public virtual HoaDon HoaDon { get; set; }

        public int SachId { get; set; }
        public virtual Sach Sach { get; set; }
    }
}
