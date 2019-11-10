namespace FRTools.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Pinglistcategorytables : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PinglistCategories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Owner_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.Owner_Id)
                .Index(t => t.Owner_Id);
            
            AddColumn("dbo.Pinglists", "PinglistCategory_Id", c => c.Int());
            CreateIndex("dbo.Pinglists", "PinglistCategory_Id");
            AddForeignKey("dbo.Pinglists", "PinglistCategory_Id", "dbo.PinglistCategories", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Pinglists", "PinglistCategory_Id", "dbo.PinglistCategories");
            DropForeignKey("dbo.PinglistCategories", "Owner_Id", "dbo.Users");
            DropIndex("dbo.Pinglists", new[] { "PinglistCategory_Id" });
            DropIndex("dbo.PinglistCategories", new[] { "Owner_Id" });
            DropColumn("dbo.Pinglists", "PinglistCategory_Id");
            DropTable("dbo.PinglistCategories");
        }
    }
}
