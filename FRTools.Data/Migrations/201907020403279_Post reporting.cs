namespace FRTools.Data.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class Postreporting : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Posts", "Reports", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Posts", "Reports");
        }
    }
}
