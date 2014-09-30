using System.Data.Entity.Migrations;

namespace DAL.Migrations
{
    public partial class UserPrimaryContactRequired : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.User", "PrimaryContact_ID", "dbo.Contact");
            DropIndex("dbo.User", new[] {"PrimaryContact_ID"});
            AlterColumn("dbo.User", "PrimaryContact_ID", c => c.Int());
            CreateIndex("dbo.User", "PrimaryContact_ID");
            AddForeignKey("dbo.User", "PrimaryContact_ID", "dbo.Contact", "ID");
        }

        public override void Down()
        {
            DropForeignKey("dbo.User", "PrimaryContact_ID", "dbo.Contact");
            DropIndex("dbo.User", new[] {"PrimaryContact_ID"});
            AlterColumn("dbo.User", "PrimaryContact_ID", c => c.Int(false));
            CreateIndex("dbo.User", "PrimaryContact_ID");
            AddForeignKey("dbo.User", "PrimaryContact_ID", "dbo.Contact", "ID", true);
        }
    }
}