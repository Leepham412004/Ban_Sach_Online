namespace Ban_Sach_Online.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateSchema : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ChiTietHoaDons", "SachId", "dbo.Saches");
            AddForeignKey("dbo.ChiTietHoaDons", "SachId", "dbo.Saches", "SachId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ChiTietHoaDons", "SachId", "dbo.Saches");
            AddForeignKey("dbo.ChiTietHoaDons", "SachId", "dbo.Saches", "SachId", cascadeDelete: true);
        }
    }
}
