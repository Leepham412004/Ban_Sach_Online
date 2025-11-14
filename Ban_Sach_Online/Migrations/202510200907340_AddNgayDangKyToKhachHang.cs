namespace Ban_Sach_Online.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddNgayDangKyToKhachHang : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.KhachHangs", "NgayDangKy", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.KhachHangs", "NgayDangKy");
        }
    }
}
