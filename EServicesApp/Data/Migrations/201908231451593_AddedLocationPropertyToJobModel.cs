namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedLocationPropertyToJobModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Jobs", "LocationId", c => c.Int(nullable: false));
            CreateIndex("dbo.Jobs", "LocationId");
            AddForeignKey("dbo.Jobs", "LocationId", "dbo.Locations", "LocationId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Jobs", "LocationId", "dbo.Locations");
            DropIndex("dbo.Jobs", new[] { "LocationId" });
            DropColumn("dbo.Jobs", "LocationId");
        }
    }
}
