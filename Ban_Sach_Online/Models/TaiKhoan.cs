using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ban_Sach_Online.Models
{
    public class TaiKhoan
    {
        public int TaiKhoanId { get; set; }
        public string TenDangNhap { get; set; }
        public string MatKhau { get; set; }

        public string VaiTro { get; set; }

        // Quan hệ
        public int? KhachHangId { get; set; }
        public virtual KhachHang KhachHang { get; set; }
    }
}
