using System.Data.Entity;
using Data.Models;

namespace Data.Config
{
    public class OmContext : DbContext
    {
        public OmContext()
            : base("EServicesConnectionString")
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<View> Views { get; set; }
        public DbSet<UserView> UserViews { get; set; }
        public DbSet<ProfileImage> ProfileImages { get; set; }
        public DbSet<Franchise> Franchises { get; set; }
        public DbSet<FranchiseAdmin> FranchiseAdmins { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Worker> Workers { get; set; }
        public DbSet<JobDetail> JobDetails { get; set; }
        public DbSet<WorkCategory> WorkCategory { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<Job> Jobs { get; set; }
        public DbSet<Location> Locations { get; set; }

        public DbSet<Payment> Payments { get; set; }
        public DbSet<NotificationDetail> NotificationDetails { get; set; }

    }
}