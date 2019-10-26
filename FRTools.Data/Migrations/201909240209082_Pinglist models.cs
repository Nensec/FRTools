namespace FRTools.Data.Migrations
{
	using System;
	using System.Collections.Generic;
	using System.Data.Entity.Migrations;
	using System.Linq;

	public partial class Pinglistmodels : DbMigration
	{
		public override void Up()
		{
			CreateTable(
				"dbo.FRUsers",
				c => new
				{
					Id = c.Int(nullable: false, identity: true),
					Username = c.String(),
					FRId = c.Int(nullable: false),
					LastUpdated = c.DateTime(nullable: false),
				})
				.PrimaryKey(t => t.Id);

			CreateTable(
				"dbo.Pinglists",
				c => new
				{
					Id = c.Int(nullable: false, identity: true),
					GeneratedId = c.String(),
					SecretKey = c.String(),
					Name = c.String(),
					IsPublic = c.Boolean(nullable: false),
					Format = c.String(),
					Creator_Id = c.Int(),
				})
				.PrimaryKey(t => t.Id)
				.ForeignKey("dbo.Users", t => t.Creator_Id)
				.Index(t => t.Creator_Id);

			CreateTable(
				"dbo.PingListEntries",
				c => new
				{
					Id = c.Int(nullable: false, identity: true),
					GeneratedId = c.String(),
					SecretKey = c.String(),
					Remarks = c.String(),
					FRUser_Id = c.Int(nullable: false),
					Pinglist_Id = c.Int(),
				})
				.PrimaryKey(t => t.Id)
				.ForeignKey("dbo.FRUsers", t => t.FRUser_Id, cascadeDelete: true)
				.ForeignKey("dbo.Pinglists", t => t.Pinglist_Id)
				.Index(t => t.FRUser_Id)
				.Index(t => t.Pinglist_Id);

			AddColumn("dbo.Users", "FRUser_Id", c => c.Int());
			CreateIndex("dbo.Users", "FRUser_Id");
			AddForeignKey("dbo.Users", "FRUser_Id", "dbo.FRUsers", "Id");

			using (var ctx = new DataContext())
			{
				var existingUsersWithFRVerify = ctx.Database.SqlQuery<int>("select id from users where frid is not null");

				foreach(var user in existingUsersWithFRVerify)
				{
					Sql($@"begin transaction 
							   declare @id int;
							   declare @frid int;
							   declare @frname varchar(max);
							   select @frid = frid, @frname = frname from users where id = {user};
							   insert into FRUsers (frid, username, lastupdated) values (@frid, @frname, getutcdate());
							   select @id = scope_identity();
							   update users set fruser_id = @id where id = {user};
						   commit");
				}
			}

			DropColumn("dbo.Users", "FRId");
			DropColumn("dbo.Users", "FRName");
		}
		
		public override void Down()
		{
			AddColumn("dbo.Users", "FRName", c => c.String());
			AddColumn("dbo.Users", "FRId", c => c.Int());
			DropForeignKey("dbo.PingListEntries", "Pinglist_Id", "dbo.Pinglists");
			DropForeignKey("dbo.PingListEntries", "FRUser_Id", "dbo.FRUsers");
			DropForeignKey("dbo.Pinglists", "Creator_Id", "dbo.Users");
			DropForeignKey("dbo.Users", "FRUser_Id", "dbo.FRUsers");
			DropIndex("dbo.PingListEntries", new[] { "Pinglist_Id" });
			DropIndex("dbo.PingListEntries", new[] { "FRUser_Id" });
			DropIndex("dbo.Pinglists", new[] { "Creator_Id" });
			DropIndex("dbo.Users", new[] { "FRUser_Id" });
			DropColumn("dbo.Users", "FRUser_Id");
			DropTable("dbo.PingListEntries");
			DropTable("dbo.Pinglists");
			DropTable("dbo.FRUsers");
		}
	}
}
