namespace FRTools.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ShareURL : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Pinglists", "ShareUrl", c => c.String());
            AddColumn("dbo.Skins", "ShareUrl", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Skins", "ShareUrl");
            DropColumn("dbo.Pinglists", "ShareUrl");
        }
    }
}
