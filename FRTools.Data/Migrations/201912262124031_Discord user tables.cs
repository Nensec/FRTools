namespace FRTools.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Discordusertables : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DiscordServerUsers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nickname = c.String(),
                        Server_Id = c.Int(),
                        User_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DiscordServers", t => t.Server_Id)
                .ForeignKey("dbo.DiscordUsers", t => t.User_Id)
                .Index(t => t.Server_Id)
                .Index(t => t.User_Id);
            
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
            
            AddColumn("dbo.DiscordServers", "IconBase64", c => c.String());
            AddColumn("dbo.DiscordRoles", "DiscordServerUser_Id", c => c.Int());
            CreateIndex("dbo.DiscordRoles", "DiscordServerUser_Id");
            AddForeignKey("dbo.DiscordRoles", "DiscordServerUser_Id", "dbo.DiscordServerUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.DiscordServerUsers", "User_Id", "dbo.DiscordUsers");
            DropForeignKey("dbo.DiscordServerUsers", "Server_Id", "dbo.DiscordServers");
            DropForeignKey("dbo.DiscordRoles", "DiscordServerUser_Id", "dbo.DiscordServerUsers");
            DropIndex("dbo.DiscordServerUsers", new[] { "User_Id" });
            DropIndex("dbo.DiscordServerUsers", new[] { "Server_Id" });
            DropIndex("dbo.DiscordRoles", new[] { "DiscordServerUser_Id" });
            DropColumn("dbo.DiscordRoles", "DiscordServerUser_Id");
            DropColumn("dbo.DiscordServers", "IconBase64");
            DropTable("dbo.DiscordUsers");
            DropTable("dbo.DiscordServerUsers");
        }
    }
}
