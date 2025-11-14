namespace Ban_Sach_Online.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateGioHangModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ChiTietGioHangs", "GiaBan", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ChiTietGioHangs", "GiaBan");
        }
    }
}
