namespace FRTools.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Addedproviderusernamecolumn : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserLogins", "ProviderUsername", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.UserLogins", "ProviderUsername");
        }
    }
}
