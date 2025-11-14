namespace Ban_Sach_Online.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddFlashSaleNgayDoi : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Saches", "LaFlashSale", c => c.Boolean(nullable: false));
            AddColumn("dbo.Saches", "LaNgayDoi", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Saches", "LaNgayDoi");
            DropColumn("dbo.Saches", "LaFlashSale");
        }
    }
}
