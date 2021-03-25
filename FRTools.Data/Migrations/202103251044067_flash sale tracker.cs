namespace FRTools.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class flashsaletracker : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.FRItemFlashSales",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DiscoveredAt = c.DateTime(nullable: false),
                        RemovedAt = c.DateTime(),
                        Item_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.FRItems", t => t.Item_Id)
                .Index(t => t.Item_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.FRItemFlashSales", "Item_Id", "dbo.FRItems");
            DropIndex("dbo.FRItemFlashSales", new[] { "Item_Id" });
            DropTable("dbo.FRItemFlashSales");
        }
    }
}
