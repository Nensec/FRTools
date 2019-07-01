namespace FRTools.Data.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class Apparelcaching : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DragonCaches", "Apparel", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.DragonCaches", "Apparel");
        }
    }
}
