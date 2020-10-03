namespace FRTools.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddIsOwnercolumn : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DiscordServerUsers", "IsOwner", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.DiscordServerUsers", "IsOwner");
        }
    }
}
