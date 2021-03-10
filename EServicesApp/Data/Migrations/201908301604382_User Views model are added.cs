namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserViewsmodelareadded : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserViews",
                c => new
                    {
                        UserViewId = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        ViewId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.UserViewId)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.Views", t => t.ViewId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.ViewId);
            
            CreateTable(
                "dbo.Views",
                c => new
                    {
                        ViewId = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                    })
                .PrimaryKey(t => t.ViewId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserViews", "ViewId", "dbo.Views");
            DropForeignKey("dbo.UserViews", "UserId", "dbo.Users");
            DropIndex("dbo.UserViews", new[] { "ViewId" });
            DropIndex("dbo.UserViews", new[] { "UserId" });
            DropTable("dbo.Views");
            DropTable("dbo.UserViews");
        }
    }
}
