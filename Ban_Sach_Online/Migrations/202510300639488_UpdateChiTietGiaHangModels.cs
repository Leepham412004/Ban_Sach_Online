namespace Ban_Sach_Online.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateChiTietGiaHangModels : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.ChiTietGioHangs", "GiaBan");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ChiTietGioHangs", "GiaBan", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
    }
}
