namespace FRTools.Data.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AddUsertoSkinandPreview : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Skins", "Creator_Id", c => c.Int());
            AddColumn("dbo.Previews", "Requestor_Id", c => c.Int());
            CreateIndex("dbo.Skins", "Creator_Id");
            CreateIndex("dbo.Previews", "Requestor_Id");
            AddForeignKey("dbo.Skins", "Creator_Id", "dbo.Users", "Id");
            AddForeignKey("dbo.Previews", "Requestor_Id", "dbo.Users", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Previews", "Requestor_Id", "dbo.Users");
            DropForeignKey("dbo.Skins", "Creator_Id", "dbo.Users");
            DropIndex("dbo.Previews", new[] { "Requestor_Id" });
            DropIndex("dbo.Skins", new[] { "Creator_Id" });
            DropColumn("dbo.Previews", "Requestor_Id");
            DropColumn("dbo.Skins", "Creator_Id");
        }
    }
}
