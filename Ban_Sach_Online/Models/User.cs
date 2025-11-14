using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Ban_Sach_Online.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string HoTen { get; set; }
        public string Email { get; set; }
        public string MatKhau { get; set; }
        public string QuyenHan { get; set; } // Admin, Nhân viên, Quản lý kho
    }
}
