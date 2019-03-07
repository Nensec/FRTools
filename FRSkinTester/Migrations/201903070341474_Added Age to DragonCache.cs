namespace FRSkinTester.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedAgetoDragonCache : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DragonCaches", "Age", c => c.Int(nullable: false, defaultValue: 1));
        }
        
        public override void Down()
        {
            DropColumn("dbo.DragonCaches", "Age");
        }
    }
}
