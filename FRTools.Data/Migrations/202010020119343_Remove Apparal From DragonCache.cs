namespace FRTools.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveApparalFromDragonCache : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.DragonCaches", "Apparel");
        }
        
        public override void Down()
        {
            AddColumn("dbo.DragonCaches", "Apparel", c => c.String());
        }
    }
}
