namespace FRTools.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Addedpreviewtable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Previews",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DragonId = c.Int(),
                        ScryerUrl = c.String(),
                        PreviewImage = c.String(),
                        DragonData = c.String(),
                        Skin_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Skins", t => t.Skin_Id)
                .Index(t => t.Skin_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Previews", "Skin_Id", "dbo.Skins");
            DropIndex("dbo.Previews", new[] { "Skin_Id" });
            DropTable("dbo.Previews");
        }
    }
}
