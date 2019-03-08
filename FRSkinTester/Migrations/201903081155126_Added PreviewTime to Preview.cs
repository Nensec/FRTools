namespace FRSkinTester.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedPreviewTimetoPreview : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Previews", "PreviewTime", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Previews", "PreviewTime");
        }
    }
}
