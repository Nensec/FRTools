namespace FRSkinTester.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Addedcachingfordragons : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DragonCaches",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DragonType = c.Int(nullable: false),
                        Gender = c.Int(nullable: false),
                        BodyGene = c.Int(nullable: false),
                        BodyColor = c.Int(nullable: false),
                        WingGene = c.Int(nullable: false),
                        WingColor = c.Int(nullable: false),
                        TertiaryGene = c.Int(nullable: false),
                        TertiaryColor = c.Int(nullable: false),
                        EyeType = c.Int(nullable: false),
                        Element = c.Int(nullable: false),
                        SHA1Hash = c.String(maxLength: 40),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.SHA1Hash);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.DragonCaches", new[] { "SHA1Hash" });
            DropTable("dbo.DragonCaches");
        }
    }
}
