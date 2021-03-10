namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedLocationModel1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Locations",
                c => new
                    {
                        LocationId = c.Int(nullable: false, identity: true),
                        Longitude = c.Double(nullable: false),
                        Latitude = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.LocationId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Locations");
        }
    }
}
