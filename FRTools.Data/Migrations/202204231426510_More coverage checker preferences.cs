namespace FRTools.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Morecoveragecheckerpreferences : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ProfileSettings", "DefaultAdvancedCoverageUseDressingRoomBase", c => c.Boolean(nullable: false, defaultValue: true));
            AddColumn("dbo.ProfileSettings", "DefaultAdvancedCoverageScry", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ProfileSettings", "DefaultAdvancedCoverageScry");
            DropColumn("dbo.ProfileSettings", "DefaultAdvancedCoverageUseDressingRoomBase");
        }
    }
}
