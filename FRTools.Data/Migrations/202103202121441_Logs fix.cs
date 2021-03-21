namespace FRTools.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Logsfix : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.DiscordLogs", "Channel_Id", "dbo.DiscordChannels");
            DropIndex("dbo.DiscordLogs", new[] { "Channel_Id" });
            AlterColumn("dbo.DiscordLogs", "Channel_Id", c => c.Int(nullable: false));
            CreateIndex("dbo.DiscordLogs", "Channel_Id");
            AddForeignKey("dbo.DiscordLogs", "Channel_Id", "dbo.DiscordChannels", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.DiscordLogs", "Channel_Id", "dbo.DiscordChannels");
            DropIndex("dbo.DiscordLogs", new[] { "Channel_Id" });
            AlterColumn("dbo.DiscordLogs", "Channel_Id", c => c.Int());
            CreateIndex("dbo.DiscordLogs", "Channel_Id");
            AddForeignKey("dbo.DiscordLogs", "Channel_Id", "dbo.DiscordChannels", "Id");
        }
    }
}
