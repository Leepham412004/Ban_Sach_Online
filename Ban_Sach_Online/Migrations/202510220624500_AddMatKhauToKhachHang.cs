namespace Ban_Sach_Online.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddMatKhauToKhachHang : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.KhachHangs", "MatKhau", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.KhachHangs", "MatKhau");
        }
    }
}
