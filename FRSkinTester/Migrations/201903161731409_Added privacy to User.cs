namespace FRSkinTester.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedprivacytoUser : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "Privacy", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "Privacy");
        }
    }
}
