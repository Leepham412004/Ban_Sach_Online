namespace Ban_Sach_Online.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AnhSaches",
                c => new
                    {
                        AnhSachId = c.Int(nullable: false, identity: true),
                        Url = c.String(),
                        SachId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.AnhSachId)
                .ForeignKey("dbo.Saches", t => t.SachId, cascadeDelete: true)
                .Index(t => t.SachId);
            
            CreateTable(
                "dbo.Saches",
                c => new
                    {
                        SachId = c.Int(nullable: false, identity: true),
                        TenSach = c.String(),
                        TacGia = c.String(),
                        Gia = c.Decimal(nullable: false, precision: 18, scale: 2),
                        SoLuong = c.Int(nullable: false),
                        MoTa = c.String(),
                        TheLoaiId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.SachId)
                .ForeignKey("dbo.TheLoais", t => t.TheLoaiId, cascadeDelete: true)
                .Index(t => t.TheLoaiId);
            
            CreateTable(
                "dbo.ChiTietHoaDons",
                c => new
                    {
                        ChiTietHoaDonId = c.Int(nullable: false, identity: true),
                        SoLuong = c.Int(nullable: false),
                        DonGia = c.Decimal(nullable: false, precision: 18, scale: 2),
                        HoaDonId = c.Int(nullable: false),
                        SachId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ChiTietHoaDonId)
                .ForeignKey("dbo.HoaDons", t => t.HoaDonId, cascadeDelete: true)
                .ForeignKey("dbo.Saches", t => t.SachId, cascadeDelete: true)
                .Index(t => t.HoaDonId)
                .Index(t => t.SachId);
            
            CreateTable(
                "dbo.HoaDons",
                c => new
                    {
                        HoaDonId = c.Int(nullable: false, identity: true),
                        NgayLap = c.DateTime(nullable: false),
                        TongTien = c.Decimal(nullable: false, precision: 18, scale: 2),
                        KhachHangId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.HoaDonId)
                .ForeignKey("dbo.KhachHangs", t => t.KhachHangId)
                .Index(t => t.KhachHangId);
            
            CreateTable(
                "dbo.KhachHangs",
                c => new
                    {
                        KhachHangId = c.Int(nullable: false, identity: true),
                        HoTen = c.String(),
                        Email = c.String(),
                        DiaChi = c.String(),
                        SoDienThoai = c.String(),
                    })
                .PrimaryKey(t => t.KhachHangId);
            
            CreateTable(
                "dbo.GioHangs",
                c => new
                    {
                        GioHangId = c.Int(nullable: false),
                        KhachHangId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.GioHangId)
                .ForeignKey("dbo.KhachHangs", t => t.GioHangId)
                .Index(t => t.GioHangId);
            
            CreateTable(
                "dbo.ChiTietGioHangs",
                c => new
                    {
                        ChiTietGioHangId = c.Int(nullable: false, identity: true),
                        SoLuong = c.Int(nullable: false),
                        GioHangId = c.Int(nullable: false),
                        SachId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ChiTietGioHangId)
                .ForeignKey("dbo.Saches", t => t.SachId, cascadeDelete: true)
                .ForeignKey("dbo.GioHangs", t => t.GioHangId, cascadeDelete: true)
                .Index(t => t.GioHangId)
                .Index(t => t.SachId);
            
            CreateTable(
                "dbo.ChiTietNhapHangs",
                c => new
                    {
                        ChiTietNhapHangId = c.Int(nullable: false, identity: true),
                        SoLuong = c.Int(nullable: false),
                        DonGia = c.Decimal(nullable: false, precision: 18, scale: 2),
                        NhapHangId = c.Int(nullable: false),
                        SachId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ChiTietNhapHangId)
                .ForeignKey("dbo.NhapHangs", t => t.NhapHangId, cascadeDelete: true)
                .ForeignKey("dbo.Saches", t => t.SachId, cascadeDelete: true)
                .Index(t => t.NhapHangId)
                .Index(t => t.SachId);
            
            CreateTable(
                "dbo.NhapHangs",
                c => new
                    {
                        NhapHangId = c.Int(nullable: false, identity: true),
                        NgayNhap = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.NhapHangId);
            
            CreateTable(
                "dbo.DanhGias",
                c => new
                    {
                        DanhGiaId = c.Int(nullable: false, identity: true),
                        NoiDung = c.String(),
                        SoSao = c.Int(nullable: false),
                        NgayDanhGia = c.DateTime(nullable: false),
                        SachId = c.Int(nullable: false),
                        KhachHangId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.DanhGiaId)
                .ForeignKey("dbo.KhachHangs", t => t.KhachHangId, cascadeDelete: true)
                .ForeignKey("dbo.Saches", t => t.SachId, cascadeDelete: true)
                .Index(t => t.SachId)
                .Index(t => t.KhachHangId);
            
            CreateTable(
                "dbo.TheLoais",
                c => new
                    {
                        TheLoaiId = c.Int(nullable: false, identity: true),
                        TenTheLoai = c.String(),
                    })
                .PrimaryKey(t => t.TheLoaiId);
            
            CreateTable(
                "dbo.TaiKhoans",
                c => new
                    {
                        TaiKhoanId = c.Int(nullable: false, identity: true),
                        TenDangNhap = c.String(),
                        MatKhau = c.String(),
                        VaiTro = c.String(),
                        KhachHangId = c.Int(),
                    })
                .PrimaryKey(t => t.TaiKhoanId)
                .ForeignKey("dbo.KhachHangs", t => t.KhachHangId)
                .Index(t => t.KhachHangId);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        UserId = c.Int(nullable: false, identity: true),
                        HoTen = c.String(),
                        Email = c.String(),
                        MatKhau = c.String(),
                        QuyenHan = c.String(),
                    })
                .PrimaryKey(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TaiKhoans", "KhachHangId", "dbo.KhachHangs");
            DropForeignKey("dbo.Saches", "TheLoaiId", "dbo.TheLoais");
            DropForeignKey("dbo.DanhGias", "SachId", "dbo.Saches");
            DropForeignKey("dbo.DanhGias", "KhachHangId", "dbo.KhachHangs");
            DropForeignKey("dbo.ChiTietNhapHangs", "SachId", "dbo.Saches");
            DropForeignKey("dbo.ChiTietNhapHangs", "NhapHangId", "dbo.NhapHangs");
            DropForeignKey("dbo.ChiTietHoaDons", "SachId", "dbo.Saches");
            DropForeignKey("dbo.HoaDons", "KhachHangId", "dbo.KhachHangs");
            DropForeignKey("dbo.GioHangs", "GioHangId", "dbo.KhachHangs");
            DropForeignKey("dbo.ChiTietGioHangs", "GioHangId", "dbo.GioHangs");
            DropForeignKey("dbo.ChiTietGioHangs", "SachId", "dbo.Saches");
            DropForeignKey("dbo.ChiTietHoaDons", "HoaDonId", "dbo.HoaDons");
            DropForeignKey("dbo.AnhSaches", "SachId", "dbo.Saches");
            DropIndex("dbo.TaiKhoans", new[] { "KhachHangId" });
            DropIndex("dbo.DanhGias", new[] { "KhachHangId" });
            DropIndex("dbo.DanhGias", new[] { "SachId" });
            DropIndex("dbo.ChiTietNhapHangs", new[] { "SachId" });
            DropIndex("dbo.ChiTietNhapHangs", new[] { "NhapHangId" });
            DropIndex("dbo.ChiTietGioHangs", new[] { "SachId" });
            DropIndex("dbo.ChiTietGioHangs", new[] { "GioHangId" });
            DropIndex("dbo.GioHangs", new[] { "GioHangId" });
            DropIndex("dbo.HoaDons", new[] { "KhachHangId" });
            DropIndex("dbo.ChiTietHoaDons", new[] { "SachId" });
            DropIndex("dbo.ChiTietHoaDons", new[] { "HoaDonId" });
            DropIndex("dbo.Saches", new[] { "TheLoaiId" });
            DropIndex("dbo.AnhSaches", new[] { "SachId" });
            DropTable("dbo.Users");
            DropTable("dbo.TaiKhoans");
            DropTable("dbo.TheLoais");
            DropTable("dbo.DanhGias");
            DropTable("dbo.NhapHangs");
            DropTable("dbo.ChiTietNhapHangs");
            DropTable("dbo.ChiTietGioHangs");
            DropTable("dbo.GioHangs");
            DropTable("dbo.KhachHangs");
            DropTable("dbo.HoaDons");
            DropTable("dbo.ChiTietHoaDons");
            DropTable("dbo.Saches");
            DropTable("dbo.AnhSaches");
        }
    }
}
