using System;
using System.Collections.Generic;
using System.Linq;

namespace Ban_Sach_Online.Models
{
    public class Sach
    {
        public int SachId { get; set; }   // Khóa chính
        public string TenSach { get; set; }
        public string TacGia { get; set; }
        public string NhaXB { get; set; }
        public int NamXB { get; set; }
        public decimal Gia { get; set; }
        public int SoLuong { get; set; }
        public string MoTa { get; set; }
        public string KichThuoc { get; set; }
        public int SoTrang {  get; set; }
        public decimal TrongLuong { get; set; }
        public bool LaNoiBat { get; set; } = false;
        public bool LaFlashSale { get; set; } = false;
        public bool LaNgayDoi { get; set; } = false;
        public bool IsSelected { get; set; }
        public int SoLuongDaBan { get; set; }
        public decimal? GiaGiam { get; set; }
        public DateTime? NgayHetHanGiamGia { get; set; }
        // Quan hệ với thể loại
        public int TheLoaiId { get; set; }
        public virtual TheLoai TheLoai { get; set; }

        // Ngày thêm sách vào hệ thống
        public DateTime NgayThem { get; set; } = DateTime.Now;

        // 1 sách có thể có nhiều ảnh
        public virtual ICollection<AnhSach> AnhSachs { get; set; } = new List<AnhSach>();

        // 1 sách có thể có nhiều đánh giá
        public virtual ICollection<DanhGia> DanhGias { get; set; } = new List<DanhGia>();

        // 1 sách có thể nằm trong nhiều chi tiết hóa đơn
        public virtual ICollection<ChiTietHoaDon> ChiTietHoaDons { get; set; } = new List<ChiTietHoaDon>();

        // 1 sách có thể nằm trong nhiều phiếu nhập hàng
        public virtual ICollection<ChiTietNhapHang> ChiTietNhapHangs { get; set; } = new List<ChiTietNhapHang>();

        public double DanhGiaTrungBinh
        {
            get
            {
                if (DanhGias != null && DanhGias.Any())
                    return Math.Round(DanhGias.Average(d => d.SoSao), 1); // làm tròn 1 chữ số thập phân
                return 0;
            }
        }
    }
}
