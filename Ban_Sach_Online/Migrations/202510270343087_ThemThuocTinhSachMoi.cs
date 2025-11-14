namespace Ban_Sach_Online.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ThemThuocTinhSachMoi : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Saches", "KichThuoc", c => c.String());
            AddColumn("dbo.Saches", "SoTrang", c => c.Int(nullable: false));
            AddColumn("dbo.Saches", "TrongLuong", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Saches", "TrongLuong");
            DropColumn("dbo.Saches", "SoTrang");
            DropColumn("dbo.Saches", "KichThuoc");
        }
    }
}
