namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedRatePropertyToJobModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Jobs", "Rate", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Jobs", "Rate");
        }
    }
}
