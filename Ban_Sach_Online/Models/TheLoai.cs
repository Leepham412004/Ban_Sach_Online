using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ban_Sach_Online.Models
{
    public class TheLoai
    {
        public int TheLoaiId { get; set; }
        public string TenTheLoai { get; set; }

        // Quan hệ
        public virtual ICollection<Sach> Sachs { get; set; }
    }
}
