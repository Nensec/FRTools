namespace FRTools.Data.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class Skincoveragefield : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Skins", "Coverage", c => c.Double());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Skins", "Coverage");
        }
    }
}
