using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ban_Sach_Online.Models
{
    public class ChiTietNhapHang
    {
        public int ChiTietNhapHangId { get; set; }
        public int SoLuong { get; set; }
        public decimal DonGia { get; set; }

        // Quan hệ
        public int NhapHangId { get; set; }
        public virtual NhapHang NhapHang { get; set; }

        public int SachId { get; set; }
        public virtual Sach Sach { get; set; }
    }
}
