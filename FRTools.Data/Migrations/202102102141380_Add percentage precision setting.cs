namespace FRTools.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Addpercentageprecisionsetting : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ProfileSettings", "DefaultAdvancedCoveragePercentagePrecision", c => c.Int(nullable: false, defaultValue: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ProfileSettings", "DefaultAdvancedCoveragePercentagePrecision");
        }
    }
}
