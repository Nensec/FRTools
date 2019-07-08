namespace FRTools.Data.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class Topicname : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Topics", "FRTopicName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Topics", "FRTopicName");
        }
    }
}
