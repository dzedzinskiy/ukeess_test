using System.Data.Entity.Migrations;

namespace DAL.Migrations
{
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Contact",
                c => new
                {
                    ID = c.Int(false, true),
                    Name = c.String(),
                    Phone = c.String(),
                    Fax = c.String(),
                    Email = c.String(),
                    Note = c.String(),
                    Discriminator = c.String(false, 128),
                })
                .PrimaryKey(t => t.ID);

            CreateTable(
                "dbo.User",
                c => new
                {
                    ID = c.Int(false, true),
                    FirstName = c.String(false),
                    LastName = c.String(false),
                    BirthDate = c.DateTime(false),
                    Sex = c.Boolean(false),
                    Married = c.Boolean(false),
                    Sallary = c.Int(false),
                    AdministrativeContact_ID = c.Int(),
                    PrimaryContact_ID = c.Int(false),
                    SecondaryContact_ID = c.Int(),
                })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Contact", t => t.AdministrativeContact_ID)
                .ForeignKey("dbo.Contact", t => t.PrimaryContact_ID, true)
                .ForeignKey("dbo.Contact", t => t.SecondaryContact_ID)
                .Index(t => t.AdministrativeContact_ID)
                .Index(t => t.PrimaryContact_ID)
                .Index(t => t.SecondaryContact_ID);
        }

        public override void Down()
        {
            DropForeignKey("dbo.User", "SecondaryContact_ID", "dbo.Contact");
            DropForeignKey("dbo.User", "PrimaryContact_ID", "dbo.Contact");
            DropForeignKey("dbo.User", "AdministrativeContact_ID", "dbo.Contact");
            DropIndex("dbo.User", new[] {"SecondaryContact_ID"});
            DropIndex("dbo.User", new[] {"PrimaryContact_ID"});
            DropIndex("dbo.User", new[] {"AdministrativeContact_ID"});
            DropTable("dbo.User");
            DropTable("dbo.Contact");
        }
    }
}