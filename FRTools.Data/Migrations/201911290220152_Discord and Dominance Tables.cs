namespace FRTools.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DiscordandDominanceTables : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DiscordChannels",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ChannelId = c.Long(nullable: false),
                        Name = c.String(),
                        Server_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DiscordServers", t => t.Server_Id)
                .Index(t => t.Server_Id);
            
            CreateTable(
                "dbo.DiscordServers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ServerId = c.Long(nullable: false),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DiscordRoles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        RoleId = c.Long(nullable: false),
                        Name = c.String(),
                        Color = c.String(),
                        DiscordPermissions = c.Long(nullable: false),
                        Server_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DiscordServers", t => t.Server_Id)
                .Index(t => t.Server_Id);
            
            CreateTable(
                "dbo.DiscordLogs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Long(nullable: false),
                        Module = c.String(),
                        Command = c.String(),
                        Data = c.String(),
                        Channel_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DiscordChannels", t => t.Channel_Id)
                .Index(t => t.Channel_Id);
            
            CreateTable(
                "dbo.DiscordSettings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Key = c.String(),
                        Value = c.String(),
                        Server_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DiscordServers", t => t.Server_Id)
                .Index(t => t.Server_Id);
            
            CreateTable(
                "dbo.FRDominances",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Timestamp = c.DateTime(nullable: false),
                        First = c.Int(nullable: false),
                        Second = c.Int(nullable: false),
                        Third = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.DiscordSettings", "Server_Id", "dbo.DiscordServers");
            DropForeignKey("dbo.DiscordLogs", "Channel_Id", "dbo.DiscordChannels");
            DropForeignKey("dbo.DiscordRoles", "Server_Id", "dbo.DiscordServers");
            DropForeignKey("dbo.DiscordChannels", "Server_Id", "dbo.DiscordServers");
            DropIndex("dbo.DiscordSettings", new[] { "Server_Id" });
            DropIndex("dbo.DiscordLogs", new[] { "Channel_Id" });
            DropIndex("dbo.DiscordRoles", new[] { "Server_Id" });
            DropIndex("dbo.DiscordChannels", new[] { "Server_Id" });
            DropTable("dbo.FRDominances");
            DropTable("dbo.DiscordSettings");
            DropTable("dbo.DiscordLogs");
            DropTable("dbo.DiscordRoles");
            DropTable("dbo.DiscordServers");
            DropTable("dbo.DiscordChannels");
        }
    }
}
