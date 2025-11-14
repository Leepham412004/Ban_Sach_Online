namespace Ban_Sach_Online.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixGioHang1to1 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.GioHangs", "KhachHangId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.GioHangs", "KhachHangId", c => c.Int(nullable: false));
        }
    }
}
