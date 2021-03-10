using System.Collections.Generic;

namespace Data.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string cnic { get; set; }
        public string phone { get; set; }
    }

    public class Role
    {
        public int RoleId { get; set; }
        public string Title { get; set; }
    }

    public class UserRole
    {
        public int UserRoleId { get; set; }
        public int UserId { get; set; }
        public virtual User User { get; set; }
        public int RoleId { get; set; }
        public virtual Role Role { get; set; }
    }
    public class View
    {
        public int ViewId { get; set; }
        public string Title { get; set; }
    }
    public class UserView
    {
        public int UserViewId { get; set; }
        public int UserId { get; set; }
        public virtual User User { get; set; }
        public int ViewId { get; set; }
        public virtual View View { get; set; }
    }
    public class ProfileImage
    {
        public int ProfileImageId { get; set; }
        public int UserId { get; set; }
        public virtual User User { get; set; }
        public string ImageUrl { get; set; }
    }
}