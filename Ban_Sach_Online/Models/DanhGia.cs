using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ban_Sach_Online.Models
{
    public class DanhGia
    {
        public int DanhGiaId { get; set; }
        public string NoiDung { get; set; }
        public int SoSao { get; set; }
        public DateTime NgayDanhGia { get; set; }

        // Quan hệ
        public int SachId { get; set; }
        public virtual Sach Sach { get; set; }

        public int KhachHangId { get; set; }
        public virtual KhachHang KhachHang { get; set; }
    }
}
