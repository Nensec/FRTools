namespace FRTools.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Ads : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ProfileSettings", "ShowAds", c => c.Boolean(nullable: false, defaultValue: true));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ProfileSettings", "ShowAds");
        }
    }
}
