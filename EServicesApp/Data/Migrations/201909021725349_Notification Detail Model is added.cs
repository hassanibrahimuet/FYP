namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NotificationDetailModelisadded : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.NotificationDetails",
                c => new
                    {
                        NotificationDetailId = c.Int(nullable: false, identity: true),
                        Token = c.String(),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.NotificationDetailId)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.NotificationDetails", "UserId", "dbo.Users");
            DropIndex("dbo.NotificationDetails", new[] { "UserId" });
            DropTable("dbo.NotificationDetails");
        }
    }
}
