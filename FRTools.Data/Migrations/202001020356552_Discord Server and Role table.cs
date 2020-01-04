namespace FRTools.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DiscordServerandRoletable : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.DiscordRoles", "DiscordServerUser_Id", "dbo.DiscordServerUsers");
            DropIndex("dbo.DiscordRoles", new[] { "DiscordServerUser_Id" });
            CreateTable(
                "dbo.DiscordServerUserDiscordRoles",
                c => new
                    {
                        DiscordServerUser_Id = c.Int(nullable: false),
                        DiscordRole_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.DiscordServerUser_Id, t.DiscordRole_Id })
                .ForeignKey("dbo.DiscordServerUsers", t => t.DiscordServerUser_Id, cascadeDelete: true)
                .ForeignKey("dbo.DiscordRoles", t => t.DiscordRole_Id, cascadeDelete: true)
                .Index(t => t.DiscordServerUser_Id)
                .Index(t => t.DiscordRole_Id);
            
            DropColumn("dbo.DiscordRoles", "DiscordServerUser_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.DiscordRoles", "DiscordServerUser_Id", c => c.Int());
            DropForeignKey("dbo.DiscordServerUserDiscordRoles", "DiscordRole_Id", "dbo.DiscordRoles");
            DropForeignKey("dbo.DiscordServerUserDiscordRoles", "DiscordServerUser_Id", "dbo.DiscordServerUsers");
            DropIndex("dbo.DiscordServerUserDiscordRoles", new[] { "DiscordRole_Id" });
            DropIndex("dbo.DiscordServerUserDiscordRoles", new[] { "DiscordServerUser_Id" });
            DropTable("dbo.DiscordServerUserDiscordRoles");
            CreateIndex("dbo.DiscordRoles", "DiscordServerUser_Id");
            AddForeignKey("dbo.DiscordRoles", "DiscordServerUser_Id", "dbo.DiscordServerUsers", "Id");
        }
    }
}
