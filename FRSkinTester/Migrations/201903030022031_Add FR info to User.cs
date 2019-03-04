namespace FRSkinTester.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddFRinfotoUser : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "FRId", c => c.Int());
            AddColumn("dbo.Users", "FRName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "FRName");
            DropColumn("dbo.Users", "FRId");
        }
    }
}
