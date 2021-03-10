namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class WorkerLocationPropertyAdded : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Workers", "Longitude", c => c.String());
            AddColumn("dbo.Workers", "Latitude", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Workers", "Latitude");
            DropColumn("dbo.Workers", "Longitude");
        }
    }
}
