using System.Data.Entity.Migrations;
using System.Linq;
using Data.Config;
using Data.Models;
using System.Collections.Generic;

namespace Data.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<OmContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(OmContext context)
        {
            if (!context.Roles.Any() && !context.Users.Any() && !context.UserRoles.Any() && !context.UserViews.Any())
            {
                var roles =
                    context.Roles.AddRange(new List<Role>
                    {
                        new Role {Title = "SuperAdmin"},
                        new Role {Title = "Franchise"},
                        new Role {Title = "Admin"},
                        new Role {Title = "Worker"},
                        new Role {Title = "Customer"}
                    });
                var views =
                context.Views.AddRange(new List<View>
                {
                        new View {Title = "Dashboard"},
                        new View {Title = "SuperAdmin"},
                        new View {Title = "Franchise"},
                        new View {Title = "Admin"},
                        new View {Title = "Worker"},
                        new View {Title = "Customer"},
                        new View {Title = "Category"},
                        new View {Title = "Services"},
                        new View {Title = "Jobs"}
                });
                context.SaveChanges();
                var newUser = context.Users.Add(
                    new User
                    {
                        Name = "Hassan Ibrahim",
                        Email = "Hassan@superadmin.com",
                        Password = "12345"
                    });
                context.UserRoles.Add(
                    new UserRole
                    {
                        UserId = newUser.UserId,
                        RoleId = roles.Where(r=>r.Title == "SuperAdmin").First().RoleId
                    });
                context.UserViews.AddRange(new List<UserView>
                {
                    new UserView
                    {
                        UserId = newUser.UserId,
                        ViewId = views.Where(r=>r.Title == "Dashboard").First().ViewId
                    },
                    new UserView
                    {
                        UserId = newUser.UserId,
                        ViewId = views.Where(r=>r.Title == "SuperAdmin").First().ViewId
                    },
                    new UserView
                    {
                        UserId = newUser.UserId,
                        ViewId = views.Where(r=>r.Title == "Franchise").First().ViewId
                    },
                });
                context.SaveChanges();
            }

            //  This method will be called after migrating to the latest version.
            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.
        }
    }
}