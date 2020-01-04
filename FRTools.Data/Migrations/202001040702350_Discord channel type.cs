namespace FRTools.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Discordchanneltype : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DiscordChannels", "ChannelType", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.DiscordChannels", "ChannelType");
        }
    }
}
