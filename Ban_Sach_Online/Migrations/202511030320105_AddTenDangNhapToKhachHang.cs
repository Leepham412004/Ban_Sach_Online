namespace Ban_Sach_Online.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTenDangNhapToKhachHang : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.KhachHangs", "TenDangNhap", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.KhachHangs", "TenDangNhap");
        }
    }
}
