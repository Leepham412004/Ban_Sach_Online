namespace Ban_Sach_Online.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTrangThaiToHoaDon : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.HoaDons", "TrangThai", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.HoaDons", "TrangThai");
        }
    }
}
