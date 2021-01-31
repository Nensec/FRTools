namespace FRTools.Data.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AdvancedCoverageSettings : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ProfileSettings", "DefaultAdvancedCoverageBackgroundColor", c => c.String(nullable: false, defaultValue: "#000000"));
            AddColumn("dbo.ProfileSettings", "DefaultAdvancedCoverageOverlayColor", c => c.String(nullable: false, defaultValue: "#FF0000"));
            AddColumn("dbo.ProfileSettings", "DefaultAdvancedCoverageDummyOpacity", c => c.Int(nullable: false, defaultValue: 40));
            AddColumn("dbo.ProfileSettings", "DefaultAdvancedCoverageSkinOpacity", c => c.Int(nullable: false, defaultValue: 40));
        }

        public override void Down()
        {
            DropColumn("dbo.ProfileSettings", "DefaultAdvancedCoverageSkinOpacity");
            DropColumn("dbo.ProfileSettings", "DefaultAdvancedCoverageDummyOpacity");
            DropColumn("dbo.ProfileSettings", "DefaultAdvancedCoverageOverlayColor");
            DropColumn("dbo.ProfileSettings", "DefaultAdvancedCoverageBackgroundColor");
        }
    }
}
