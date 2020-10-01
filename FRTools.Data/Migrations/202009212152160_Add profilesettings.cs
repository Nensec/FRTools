namespace FRTools.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Addprofilesettings : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ProfileSettings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ProfileBio = c.String(),
                        PublicProfile = c.Boolean(nullable: false),
                        ShowPreviewsOnProfile = c.Boolean(nullable: false),
                        ShowSkinsOnProfile = c.Boolean(nullable: false),
                        ShowPingListsOnProfile = c.Boolean(nullable: false),
                        DefaultShowSkinsInBrowse = c.Boolean(nullable: false),
                        DefaultSkinsArePublic = c.Boolean(nullable: false),
                        ShowFRLinkStatus = c.Boolean(nullable: false),
                        User_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.User_Id)
                .Index(t => t.User_Id);

            Sql(@"INSERT INTO ProfileSettings (User_Id, PublicProfile, ShowSkinsOnProfile, ShowPreviewsOnProfile, ShowPingListsOnProfile, DefaultShowSkinsInBrowse, DefaultSkinsArePublic, ShowFRLinkStatus)
                  SELECT Id,
                  CASE
                      WHEN Privacy = 3 THEN 0
                      ELSE 1
                  END AS PublicProfile,
                  CASE
                      WHEN Privacy < 3 THEN 1
                      ELSE 0
                  END AS ShowSkinsOnProfile,
                  CASE
                      WHEN Privacy <> 1 AND Privacy <> 3 THEN 1
                      ELSE 0
                  END AS ShowSkinsOnProfile,
                  CASE
                      WHEN Privacy <> 3 THEN 1
                      ELSE 0
                  END AS ShowPingListsOnProfile,
                  CASE
                      WHEN DefaultVisibility = 0 THEN 1
                      ELSE 0
                  END AS DefaultShowSkinsInBrowse,
                  CASE
                      WHEN DefaultVisibility < 2 THEN 1
                      ELSE 0
                  END AS DefaultSkinsArePublic,
                  CASE
                      WHEN Privacy < 2 THEN 1
                      ELSE 0
                  END AS ShowFRLinkStatus
                  FROM Users");

            DropColumn("dbo.Users", "Privacy");
            DropColumn("dbo.Users", "DefaultVisibility");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Users", "DefaultVisibility", c => c.Int(nullable: false));
            AddColumn("dbo.Users", "Privacy", c => c.Int(nullable: false));
            DropForeignKey("dbo.ProfileSettings", "User_Id", "dbo.Users");
            DropIndex("dbo.ProfileSettings", new[] { "User_Id" });
            DropTable("dbo.ProfileSettings");
        }
    }
}
