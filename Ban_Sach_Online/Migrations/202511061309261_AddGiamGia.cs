namespace Ban_Sach_Online.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddGiamGia : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Saches", "GiaGiam", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.Saches", "NgayHetHanGiamGia", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Saches", "NgayHetHanGiamGia");
            DropColumn("dbo.Saches", "GiaGiam");
        }
    }
}
