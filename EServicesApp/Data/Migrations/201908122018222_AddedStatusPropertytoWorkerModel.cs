namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedStatusPropertytoWorkerModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Workers", "status", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Workers", "status");
        }
    }
}
