namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedVisitPropertytoJobModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Jobs", "Visit", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Jobs", "Visit");
        }
    }
}
