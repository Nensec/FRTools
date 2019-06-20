namespace FRTools.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Skinstable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Skins",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Description = c.String(),
                        DragonType = c.Int(nullable: false),
                        GenderType = c.Int(nullable: false),
                        GeneratedId = c.String(),
                        SecretKey = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Skins");
        }
    }
}
