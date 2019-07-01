namespace FRTools.Data.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class Skinversioning : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Skins", "Version", c => c.Int(nullable: false, defaultValue: 1));
            AddColumn("dbo.Previews", "Version", c => c.Int(nullable: false, defaultValue: 1));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Previews", "Version");
            DropColumn("dbo.Skins", "Version");
        }
    }
}
