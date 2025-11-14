namespace Ban_Sach_Online.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addSachNoiBat : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Saches", "LaNoiBat", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Saches", "LaNoiBat");
        }
    }
}
