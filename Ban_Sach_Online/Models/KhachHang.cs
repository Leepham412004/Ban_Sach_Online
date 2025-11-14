using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ban_Sach_Online.Models
{
    public class KhachHang
    {
        public int KhachHangId { get; set; }
        public string HoTen { get; set; }
        public string Email { get; set; }
        public string TenDangNhap { get; set; }

        public string MatKhau { get; set; }
        public string DiaChi { get; set; }
        public string SoDienThoai { get; set; }
        public DateTime NgayDangKy { get; set; }

        // Quan hệ
        public virtual ICollection<HoaDon> HoaDons { get; set; }
        public virtual GioHang GioHang { get; set; }
    }
}
