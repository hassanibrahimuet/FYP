namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BasicModelsareadded : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Addresses",
                c => new
                    {
                        AddressId = c.Int(nullable: false, identity: true),
                        Country = c.String(),
                        State = c.String(),
                        Street = c.String(),
                        House = c.String(),
                        Longitude = c.String(),
                        Latitude = c.String(),
                    })
                .PrimaryKey(t => t.AddressId);
            
            CreateTable(
                "dbo.Customers",
                c => new
                    {
                        CustomerId = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        AddressId = c.Int(nullable: false),
                        FranchiseId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.CustomerId)
                .ForeignKey("dbo.Addresses", t => t.AddressId)
                .ForeignKey("dbo.Franchises", t => t.FranchiseId)
                .ForeignKey("dbo.Users", t => t.UserId)
                .Index(t => t.UserId)
                .Index(t => t.AddressId)
                .Index(t => t.FranchiseId);
            
            CreateTable(
                "dbo.Franchises",
                c => new
                    {
                        FranchiseId = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.FranchiseId)
                .ForeignKey("dbo.Users", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.FranchiseAdmins",
                c => new
                    {
                        FranchiseAdminId = c.Int(nullable: false, identity: true),
                        FranchiseId = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.FranchiseAdminId)
                .ForeignKey("dbo.Franchises", t => t.FranchiseId)
                .ForeignKey("dbo.Users", t => t.UserId)
                .Index(t => t.FranchiseId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Jobs",
                c => new
                    {
                        JobId = c.Int(nullable: false, identity: true),
                        ServiceId = c.Int(nullable: false),
                        CustomerId = c.Int(nullable: false),
                        WorkerId = c.Int(nullable: false),
                        RequestedOn = c.DateTime(nullable: false),
                        CompletedOn = c.DateTime(),
                        status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.JobId)
                .ForeignKey("dbo.Customers", t => t.CustomerId)
                .ForeignKey("dbo.Services", t => t.ServiceId)
                .ForeignKey("dbo.Workers", t => t.WorkerId)
                .Index(t => t.ServiceId)
                .Index(t => t.CustomerId)
                .Index(t => t.WorkerId);
            
            CreateTable(
                "dbo.Services",
                c => new
                    {
                        ServiceId = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Price = c.Double(nullable: false),
                        WorkCategoryId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ServiceId)
                .ForeignKey("dbo.WorkCategories", t => t.WorkCategoryId)
                .Index(t => t.WorkCategoryId);
            
            CreateTable(
                "dbo.WorkCategories",
                c => new
                    {
                        WorkCategoryId = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        FranchiseId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.WorkCategoryId)
                .ForeignKey("dbo.Franchises", t => t.FranchiseId)
                .Index(t => t.FranchiseId);
            
            CreateTable(
                "dbo.Workers",
                c => new
                    {
                        WorkerId = c.Int(nullable: false, identity: true),
                        FranchiseId = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                        WorkCategoryId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.WorkerId)
                .ForeignKey("dbo.Franchises", t => t.FranchiseId)
                .ForeignKey("dbo.Users", t => t.UserId)
                .ForeignKey("dbo.WorkCategories", t => t.WorkCategoryId)
                .Index(t => t.FranchiseId)
                .Index(t => t.UserId)
                .Index(t => t.WorkCategoryId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Jobs", "WorkerId", "dbo.Workers");
            DropForeignKey("dbo.Workers", "WorkCategoryId", "dbo.WorkCategories");
            DropForeignKey("dbo.Workers", "UserId", "dbo.Users");
            DropForeignKey("dbo.Workers", "FranchiseId", "dbo.Franchises");
            DropForeignKey("dbo.Jobs", "ServiceId", "dbo.Services");
            DropForeignKey("dbo.Services", "WorkCategoryId", "dbo.WorkCategories");
            DropForeignKey("dbo.WorkCategories", "FranchiseId", "dbo.Franchises");
            DropForeignKey("dbo.Jobs", "CustomerId", "dbo.Customers");
            DropForeignKey("dbo.FranchiseAdmins", "UserId", "dbo.Users");
            DropForeignKey("dbo.FranchiseAdmins", "FranchiseId", "dbo.Franchises");
            DropForeignKey("dbo.Customers", "UserId", "dbo.Users");
            DropForeignKey("dbo.Customers", "FranchiseId", "dbo.Franchises");
            DropForeignKey("dbo.Franchises", "UserId", "dbo.Users");
            DropForeignKey("dbo.Customers", "AddressId", "dbo.Addresses");
            DropIndex("dbo.Workers", new[] { "WorkCategoryId" });
            DropIndex("dbo.Workers", new[] { "UserId" });
            DropIndex("dbo.Workers", new[] { "FranchiseId" });
            DropIndex("dbo.WorkCategories", new[] { "FranchiseId" });
            DropIndex("dbo.Services", new[] { "WorkCategoryId" });
            DropIndex("dbo.Jobs", new[] { "WorkerId" });
            DropIndex("dbo.Jobs", new[] { "CustomerId" });
            DropIndex("dbo.Jobs", new[] { "ServiceId" });
            DropIndex("dbo.FranchiseAdmins", new[] { "UserId" });
            DropIndex("dbo.FranchiseAdmins", new[] { "FranchiseId" });
            DropIndex("dbo.Franchises", new[] { "UserId" });
            DropIndex("dbo.Customers", new[] { "FranchiseId" });
            DropIndex("dbo.Customers", new[] { "AddressId" });
            DropIndex("dbo.Customers", new[] { "UserId" });
            DropTable("dbo.Workers");
            DropTable("dbo.WorkCategories");
            DropTable("dbo.Services");
            DropTable("dbo.Jobs");
            DropTable("dbo.FranchiseAdmins");
            DropTable("dbo.Franchises");
            DropTable("dbo.Customers");
            DropTable("dbo.Addresses");
        }
    }
}
