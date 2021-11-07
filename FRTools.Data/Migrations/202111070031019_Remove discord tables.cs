namespace FRTools.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Removediscordtables : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.DiscordLogs", "Channel_Id", "dbo.DiscordChannels");
            DropForeignKey("dbo.DiscordChannels", "Server_Id", "dbo.DiscordServers");
            DropForeignKey("dbo.DiscordRoles", "Server_Id", "dbo.DiscordServers");
            DropForeignKey("dbo.DiscordServerUserDiscordRoles", "DiscordServerUser_Id", "dbo.DiscordServerUsers");
            DropForeignKey("dbo.DiscordServerUserDiscordRoles", "DiscordRole_Id", "dbo.DiscordRoles");
            DropForeignKey("dbo.DiscordServerUsers", "Server_Id", "dbo.DiscordServers");
            DropForeignKey("dbo.DiscordServerUsers", "User_Id", "dbo.DiscordUsers");
            DropIndex("dbo.DiscordChannels", new[] { "Server_Id" });
            DropIndex("dbo.DiscordLogs", new[] { "Channel_Id" });
            DropIndex("dbo.DiscordRoles", new[] { "Server_Id" });
            DropIndex("dbo.DiscordServerUsers", new[] { "Server_Id" });
            DropIndex("dbo.DiscordServerUsers", new[] { "User_Id" });
            DropIndex("dbo.DiscordServerUserDiscordRoles", new[] { "DiscordServerUser_Id" });
            DropIndex("dbo.DiscordServerUserDiscordRoles", new[] { "DiscordRole_Id" });
            AddColumn("dbo.DiscordLogs", "ChannelId", c => c.Long(nullable: false));
            DropColumn("dbo.DiscordLogs", "Channel_Id");
            DropTable("dbo.DiscordChannels");
            DropTable("dbo.DiscordRoles");
            DropTable("dbo.DiscordServerUsers");
            DropTable("dbo.DiscordUsers");
            DropTable("dbo.DiscordServerUserDiscordRoles");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.DiscordServerUserDiscordRoles",
                c => new
                    {
                        DiscordServerUser_Id = c.Int(nullable: false),
                        DiscordRole_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.DiscordServerUser_Id, t.DiscordRole_Id });
            
            CreateTable(
                "dbo.DiscordUsers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Username = c.String(),
                        Discriminator = c.Int(nullable: false),
                        UserId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DiscordServerUsers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nickname = c.String(),
                        IsOwner = c.Boolean(nullable: false),
                        Server_Id = c.Int(),
                        User_Id = c.Int(),
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
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DiscordChannels",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ChannelId = c.Long(nullable: false),
                        Name = c.String(),
                        ChannelType = c.Int(nullable: false),
                        Server_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.DiscordLogs", "Channel_Id", c => c.Int(nullable: false));
            DropColumn("dbo.DiscordLogs", "ChannelId");
            CreateIndex("dbo.DiscordServerUserDiscordRoles", "DiscordRole_Id");
            CreateIndex("dbo.DiscordServerUserDiscordRoles", "DiscordServerUser_Id");
            CreateIndex("dbo.DiscordServerUsers", "User_Id");
            CreateIndex("dbo.DiscordServerUsers", "Server_Id");
            CreateIndex("dbo.DiscordRoles", "Server_Id");
            CreateIndex("dbo.DiscordLogs", "Channel_Id");
            CreateIndex("dbo.DiscordChannels", "Server_Id");
            AddForeignKey("dbo.DiscordServerUsers", "User_Id", "dbo.DiscordUsers", "Id");
            AddForeignKey("dbo.DiscordServerUsers", "Server_Id", "dbo.DiscordServers", "Id");
            AddForeignKey("dbo.DiscordServerUserDiscordRoles", "DiscordRole_Id", "dbo.DiscordRoles", "Id", cascadeDelete: true);
            AddForeignKey("dbo.DiscordServerUserDiscordRoles", "DiscordServerUser_Id", "dbo.DiscordServerUsers", "Id", cascadeDelete: true);
            AddForeignKey("dbo.DiscordRoles", "Server_Id", "dbo.DiscordServers", "Id");
            AddForeignKey("dbo.DiscordChannels", "Server_Id", "dbo.DiscordServers", "Id");
            AddForeignKey("dbo.DiscordLogs", "Channel_Id", "dbo.DiscordChannels", "Id", cascadeDelete: true);
        }
    }
}
