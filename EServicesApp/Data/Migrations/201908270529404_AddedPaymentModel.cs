namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedPaymentModel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Payments",
                c => new
                    {
                        PaymentId = c.Int(nullable: false, identity: true),
                        JobId = c.Int(nullable: false),
                        PaidAmount = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.PaymentId)
                .ForeignKey("dbo.Jobs", t => t.JobId, cascadeDelete: true)
                .Index(t => t.JobId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Payments", "JobId", "dbo.Jobs");
            DropIndex("dbo.Payments", new[] { "JobId" });
            DropTable("dbo.Payments");
        }
    }
}
