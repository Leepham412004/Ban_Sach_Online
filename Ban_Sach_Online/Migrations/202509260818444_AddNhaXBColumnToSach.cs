namespace Ban_Sach_Online.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddNhaXBColumnToSach : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Saches", "NhaXB", c => c.String());
            AddColumn("dbo.Saches", "NamXB", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Saches", "NamXB");
            DropColumn("dbo.Saches", "NhaXB");
        }
    }
}
