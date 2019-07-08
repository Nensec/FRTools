namespace FRTools.Data.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class Newsreadermodels : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Posts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FRPostId = c.Int(nullable: false),
                        Content = c.String(),
                        PostAuthor = c.String(),
                        PostAuthorClanId = c.Int(nullable: false),
                        Deleted = c.Boolean(nullable: false),
                        TimeStamp = c.DateTime(nullable: false),
                        Topic_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Topics", t => t.Topic_Id)
                .Index(t => t.FRPostId)
                .Index(t => t.Topic_Id);
            
            CreateTable(
                "dbo.Topics",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FRTopicId = c.Int(nullable: false),
                        TopicStarter = c.String(),
                        TopicStarterClanId = c.Int(nullable: false),
                        FRClaimedReplyCount = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.FRTopicId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Posts", "Topic_Id", "dbo.Topics");
            DropIndex("dbo.Topics", new[] { "FRTopicId" });
            DropIndex("dbo.Posts", new[] { "Topic_Id" });
            DropIndex("dbo.Posts", new[] { "FRPostId" });
            DropTable("dbo.Topics");
            DropTable("dbo.Posts");
        }
    }
}
