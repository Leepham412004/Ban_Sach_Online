namespace Ban_Sach_Online.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SachDaBan : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Saches", "SoLuongDaBan", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Saches", "SoLuongDaBan");
        }
    }
}
