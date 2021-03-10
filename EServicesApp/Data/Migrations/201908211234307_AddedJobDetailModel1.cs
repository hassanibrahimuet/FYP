namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedJobDetailModel1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.JobDetails",
                c => new
                    {
                        JobDetailId = c.Int(nullable: false, identity: true),
                        JobId = c.Int(nullable: false),
                        StartedOn = c.DateTime(nullable: false),
                        FinishedOn = c.DateTime(),
                        status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.JobDetailId)
                .ForeignKey("dbo.Jobs", t => t.JobId, cascadeDelete: true)
                .Index(t => t.JobId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.JobDetails", "JobId", "dbo.Jobs");
            DropIndex("dbo.JobDetails", new[] { "JobId" });
            DropTable("dbo.JobDetails");
        }
    }
}
