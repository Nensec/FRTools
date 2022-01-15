namespace FRTools.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FRItemCreator : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.FRItems", "Creator_Id", c => c.Int());
            CreateIndex("dbo.FRItems", "Creator_Id");
            AddForeignKey("dbo.FRItems", "Creator_Id", "dbo.FRUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.FRItems", "Creator_Id", "dbo.FRUsers");
            DropIndex("dbo.FRItems", new[] { "Creator_Id" });
            DropColumn("dbo.FRItems", "Creator_Id");
        }
    }
}
