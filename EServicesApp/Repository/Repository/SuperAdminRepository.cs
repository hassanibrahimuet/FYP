using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Data.Config;
using Data.Models;
using Repository.DTOModel;

namespace Repository.Repository
{
    public class SuperAdminRepository
    {
        public List<SimpleUserDto> GetAllSuperAdmins()
        {
            using (var dbContext = new OmContext())
            {
                var superAdmins = (from u in dbContext.Users
                    join ur in dbContext.UserRoles on u.UserId equals ur.UserId
                    join r in dbContext.Roles on ur.RoleId equals r.RoleId
                    where r.Title=="SuperAdmin"
                    select u).ToList();
                return Mapper.Map<List<SimpleUserDto>>(superAdmins);
            }
        }

        public SimpleUserDto GetSuperAdmin(int userId)
        {
            using (var dbContext = new OmContext())
            {
                var superAdmins = (from u in dbContext.Users
                    join ur in dbContext.UserRoles on u.UserId equals ur.UserId
                    join r in dbContext.Roles on ur.RoleId equals r.RoleId
                    where r.Title == "SuperAdmin" && u.UserId == userId
                    select u).FirstOrDefault();
                return Mapper.Map<SimpleUserDto>(superAdmins);
            }
        }

        public bool DeleteSuperAdmin(int userId)
        {
            using (var dbContext = new OmContext())
            {
                var user = dbContext.Users.FirstOrDefault(u => u.UserId == userId);
                if (user != null)
                {
                    dbContext.Users.Remove(user);
                    dbContext.SaveChanges();
                    return true;
                }

                return false;
            }
        }

        public SimpleUserDto AddSuperAdmin(UserDto fullUserDto)
        {
            using (var dbContext = new OmContext())
            {
                try
                {
                    var user = dbContext.Users.FirstOrDefault(a => a.Email == fullUserDto.Email);
                    if (user == null)
                    {

                        User newUser = new User()
                        {
                            Name = fullUserDto.Name,
                            Email = fullUserDto.Email,
                            Password = fullUserDto.Password,
                            cnic = fullUserDto.cnic,
                        };
                        dbContext.Users.Add(newUser);
                        var role = dbContext.Roles.FirstOrDefault(r => r.Title == "SuperAdmin");
                        if (role != null)
                        {
                            var userRole = new UserRole
                            {
                                UserId = newUser.UserId,
                                RoleId = role.RoleId
                            };
                            dbContext.UserRoles.Add(userRole);
                            var userView = new List<UserView>
                            {
                                new UserView { UserId=newUser.UserId,ViewId=1},
                                new UserView { UserId=newUser.UserId,ViewId=2},
                                new UserView { UserId=newUser.UserId,ViewId=3}
                            };
                            dbContext.UserViews.AddRange(userView);
                            dbContext.SaveChanges();
                            return Mapper.Map<SimpleUserDto>(newUser);
                        }
                    }
                    return null;
                }
                catch (Exception e)
                {
                    return null;
                }
            }
        }

        public SimpleUserDto UpdateSuperAdmin(int userId, UserDto fullUserDto)
        {
            using (var dbContext = new OmContext())
            {
                try
                {
                    var user = dbContext.Users.FirstOrDefault(a => a.UserId == userId);
                    if (user != null)
                    {
                        user.Name = fullUserDto.Name;
                        user.Email = fullUserDto.Email;
                        user.Password = fullUserDto.Password;
                        user.cnic = fullUserDto.cnic;
                        dbContext.SaveChanges();
                        return Mapper.Map<SimpleUserDto>(user);
                    }
                    return null;
                }
                catch (Exception e)
                {
                    return null;
                }
            }
        }
    }
}
