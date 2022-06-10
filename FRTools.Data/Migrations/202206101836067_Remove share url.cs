namespace FRTools.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Removeshareurl : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Pinglists", "ShareUrl");
            DropColumn("dbo.Skins", "ShareUrl");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Skins", "ShareUrl", c => c.String());
            AddColumn("dbo.Pinglists", "ShareUrl", c => c.String());
        }
    }
}
