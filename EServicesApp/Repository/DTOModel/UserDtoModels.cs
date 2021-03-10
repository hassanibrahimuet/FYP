using System.Collections.Generic;

namespace Repository.DTOModel
{
    public class SimpleUserDto
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string cnic { get; set; }
        public string phone { get; set; }
    }
    public class WorkerUserDto : SimpleUserDto
    {
        public int WorkCategoryId { get; set; }
    }

    public class UserDto:SimpleUserDto
    {
        public List<RoleDto> UserRoles { get; set; }
    }

    public class RoleDto
    {
        public int RoleId { get; set; }
        public string Title { get; set; }
    }
    public class ViewDto
    {
        public int ViewId { get; set; }
        public string Title { get; set; }
    }
    public class UserRoleDto
    {
        public int UserRoleId { get; set; }
        public int UserId { get; set; }
        public int RoleId { get; set; }
    }
    public class FullUserDto:UserDto
    {
        public List<RoleDto> UserRoles { get; set; }
        public List<ViewDto> UserViews { get; set; }
    }
}