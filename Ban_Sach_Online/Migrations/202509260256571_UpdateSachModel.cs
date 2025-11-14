namespace Ban_Sach_Online.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateSachModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Saches", "NgayThem", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Saches", "NgayThem");
        }
    }
}
