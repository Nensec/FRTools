namespace FRTools.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FRItemtable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.FRItems",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FRId = c.Int(nullable: false),
                        Name = c.String(),
                        Description = c.String(),
                        ItemCategory = c.Int(nullable: false),
                        IconUrl = c.String(),
                        Rarity = c.Int(),
                        AssetUrl = c.String(),
                        TreasureValue = c.Int(),
                        ItemType = c.String(),
                        FoodType = c.Int(),
                        FoodValue = c.Int(),
                        RequiredLevel = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.FRItems");
        }
    }
}
