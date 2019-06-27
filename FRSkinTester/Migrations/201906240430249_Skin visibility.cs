namespace FRSkinTester.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Skinvisibility : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Skins", "Visibility", c => c.Int(nullable: false));
            AddColumn("dbo.Users", "DefaultVisibility", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "DefaultVisibility");
            DropColumn("dbo.Skins", "Visibility");
        }
    }
}
