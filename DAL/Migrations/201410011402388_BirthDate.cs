namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BirthDate : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.User", "BirthDate", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.User", "BirthDate", c => c.DateTime(nullable: false));
        }
    }
}
