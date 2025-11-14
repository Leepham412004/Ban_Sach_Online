using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ban_Sach_Online.Models
{
    public class NhapHang
    {
        public int NhapHangId { get; set; }
        public DateTime NgayNhap { get; set; }

        public virtual ICollection<ChiTietNhapHang> ChiTietNhapHangs { get; set; }
    }
}
