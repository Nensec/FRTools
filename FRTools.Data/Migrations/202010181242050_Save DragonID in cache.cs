namespace FRTools.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SaveDragonIDincache : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DragonCaches", "FRDragonId", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.DragonCaches", "FRDragonId");
        }
    }
}
