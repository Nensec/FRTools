namespace FRTools.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Adderrorfieldtojob : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Jobs", "Errors", c => c.String());
            Sql("update dbo.Jobs set status = 5 where status = 1");
        }
        
        public override void Down()
        {
            DropColumn("dbo.Jobs", "Errors");
            Sql("update dbo.Jobs set status = 1 where status = 5");
        }
    }
}
