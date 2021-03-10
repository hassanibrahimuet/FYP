using System.Data.Entity.Migrations;

namespace Data.Migrations
{
    public partial class Userrelatedmodelsareadded : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                    "dbo.Roles",
                    c => new
                    {
                        RoleId = c.Int(false, true),
                        Title = c.String()
                    })
                .PrimaryKey(t => t.RoleId);

            CreateTable(
                    "dbo.UserRoles",
                    c => new
                    {
                        UserRoleId = c.Int(false, true),
                        UserId = c.Int(false),
                        RoleId = c.Int(false)
                    })
                .PrimaryKey(t => t.UserRoleId)
                .ForeignKey("dbo.Roles", t => t.RoleId)
                .ForeignKey("dbo.Users", t => t.UserId)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);

            CreateTable(
                    "dbo.Users",
                    c => new
                    {
                        UserId = c.Int(false, true),
                        Name = c.String(),
                        Email = c.String(),
                        Password = c.String(),
                        cnic = c.String()
                    })
                .PrimaryKey(t => t.UserId);
        }

        public override void Down()
        {
            DropForeignKey("dbo.UserRoles", "UserId", "dbo.Users");
            DropForeignKey("dbo.UserRoles", "RoleId", "dbo.Roles");
            DropIndex("dbo.UserRoles", new[] {"RoleId"});
            DropIndex("dbo.UserRoles", new[] {"UserId"});
            DropTable("dbo.Users");
            DropTable("dbo.UserRoles");
            DropTable("dbo.Roles");
        }
    }
}