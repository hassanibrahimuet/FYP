namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedRatePropertyToWorkerModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Workers", "Rate", c => c.Single(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Workers", "Rate");
        }
    }
}
