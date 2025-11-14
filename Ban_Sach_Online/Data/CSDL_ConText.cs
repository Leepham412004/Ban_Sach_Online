using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using Ban_Sach_Online.Models;

namespace Ban_Sach_Online.Data
{
    public class CSDL_Context : DbContext
    {
        public CSDL_Context() : base("Ban_Sach_OnlineDB")
        {
        }

        // Các DbSet (bảng trong CSDL)
        public DbSet<Sach> Sachs { get; set; }
        public DbSet<AnhSach> AnhSachs { get; set; }
        public DbSet<TheLoai> TheLoais { get; set; }
        public DbSet<KhachHang> KhachHangs { get; set; }
        public DbSet<TaiKhoan> TaiKhoans { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<HoaDon> HoaDons { get; set; }
        public DbSet<ChiTietHoaDon> ChiTietHoaDons { get; set; }
        public DbSet<NhapHang> NhapHangs { get; set; }
        public DbSet<ChiTietNhapHang> ChiTietNhapHangs { get; set; }
        public DbSet<GioHang> GioHangs { get; set; }
        public DbSet<ChiTietGioHang> ChiTietGioHangs { get; set; }
        public DbSet<DanhGia> DanhGias { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ---- KhachHang 1-1 GioHang ----
            modelBuilder.Entity<KhachHang>()
                .HasOptional(k => k.GioHang)      // KhachHang có thể có hoặc chưa có GioHang
                .WithRequired(g => g.KhachHang);  // GioHang bắt buộc có KhachHang

            // ---- KhachHang 1-n HoaDon ----
            modelBuilder.Entity<KhachHang>()
                .HasMany(k => k.HoaDons)
                .WithRequired(h => h.KhachHang)
                .HasForeignKey(h => h.KhachHangId)
                .WillCascadeOnDelete(false); // Không xóa các Hóa đơn khi xóa Khách hàng

            // ---- GioHang 1-n ChiTietGioHang ----
            modelBuilder.Entity<GioHang>()
                .HasMany(g => g.ChiTietGioHangs)
                .WithRequired(c => c.GioHang)
                .HasForeignKey(c => c.GioHangId)
                .WillCascadeOnDelete(true);

            // ---- HoaDon 1-n ChiTietHoaDon ----
            modelBuilder.Entity<HoaDon>()
                .HasMany(h => h.ChiTietHoaDons)
                .WithRequired(c => c.HoaDon)
                .HasForeignKey(c => c.HoaDonId)
                .WillCascadeOnDelete(true);
            // ---- Sach 1-n ChiTietHoaDon ----
            modelBuilder.Entity<Sach>()
                .HasMany(s => s.ChiTietHoaDons)
                .WithRequired(ct => ct.Sach)
                .HasForeignKey(ct => ct.SachId)
                .WillCascadeOnDelete(false);
        }

    }
}
