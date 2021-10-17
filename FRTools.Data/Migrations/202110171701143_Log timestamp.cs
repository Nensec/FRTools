namespace FRTools.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Logtimestamp : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DiscordLogs", "CreatedAt", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.DiscordLogs", "CreatedAt");
        }
    }
}
