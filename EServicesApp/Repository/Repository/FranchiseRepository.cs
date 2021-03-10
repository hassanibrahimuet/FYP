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
    public class FranchiseRepository
    {
        public List<FranchiseUserDto> GetAllFranchises()
        {
            using (var dbContext = new OmContext())
            {
                var franchises = (from f in dbContext.Franchises
                    join u in dbContext.Users on f.UserId equals u.UserId
                    join ur in dbContext.UserRoles on u.UserId equals ur.UserId
                    join r in dbContext.Roles on ur.RoleId equals r.RoleId
                    where r.Title == "Franchise"
                    select new FranchiseUserDto
                    {
                        FranchiseId = f.FranchiseId,
                        FranchiseName = f.Name,
                        FranchiseUser = new SimpleUserDto
                        {
                            UserId = u.UserId,
                            Name = u.Name,
                            Email = u.Email,
                            Password = u.Password,
                            cnic = u.cnic
                        }
                    }).ToList();
                return franchises;
            }
        }
        public FranchiseUserDto GetFranchise(int franchiseId)
        {
            using (var dbContext = new OmContext())
            {
                var franchises = (from f in dbContext.Franchises
                    join u in dbContext.Users on f.UserId equals u.UserId
                    join ur in dbContext.UserRoles on u.UserId equals ur.UserId
                    join r in dbContext.Roles on ur.RoleId equals r.RoleId
                    where r.Title == "Franchise" && f.FranchiseId==franchiseId
                    select new FranchiseUserDto
                    {
                        FranchiseId = f.FranchiseId,
                        FranchiseName = f.Name,
                        FranchiseUser = new SimpleUserDto
                        {
                            UserId = u.UserId,
                            Name = u.Name,
                            Email = u.Email,
                            Password = u.Password,
                            cnic = u.cnic
                        }
                    }).FirstOrDefault();
                return franchises;
            }
        }

        public bool DeleteFranchise(int franchiseId)
        {
            using (var dbContext = new OmContext())
            {
                try
                {
                    var franchise = dbContext.Franchises.FirstOrDefault(u => u.FranchiseId == franchiseId);
                if (franchise != null)
                {
                    var users = dbContext.Users.FirstOrDefault(u => u.UserId == franchise.UserId);
                    var roles = dbContext.UserRoles.FirstOrDefault(ur => ur.UserId == franchise.UserId);
                    dbContext.Franchises.Remove(franchise);
                    dbContext.UserRoles.Remove(roles);
                    dbContext.Users.Remove(users);
                    dbContext.SaveChanges();
                    return true;
                }
                return false;
                }
                catch (Exception e)
                {
                    return false;
                }
            }
        }

        public FranchiseUserDto AddFranchise(FranchiseUserDto franchiseDto)
        {
            using (var dbContext = new OmContext())
            {
                try
                {
                    var franchise = dbContext.Franchises.FirstOrDefault(f => f.FranchiseId == franchiseDto.FranchiseId);
                    if (franchise == null)
                    {

                        var user = dbContext.Users.FirstOrDefault(a => a.Email == franchiseDto.FranchiseUser.Email);
                        if (user == null)
                        {
                            User newUser = new User()
                            {
                                Name = franchiseDto.FranchiseUser.Name,
                                Email = franchiseDto.FranchiseUser.Email,
                                Password = franchiseDto.FranchiseUser.Password,
                                cnic = franchiseDto.FranchiseUser.cnic,
                            };
                            dbContext.Users.Add(newUser);
                            var newFranchise = dbContext.Franchises.Add(new Franchise
                            {
                                Name = franchiseDto.FranchiseName,
                                UserId=newUser.UserId
                            });
                            var role = dbContext.Roles.FirstOrDefault(r => r.Title == "Franchise");
                            if (role != null)
                            {
                                var userRole = new UserRole
                                {
                                    UserId = newUser.UserId,
                                    RoleId = role.RoleId
                                };
                                dbContext.UserRoles.Add(userRole);
                                var allViews = dbContext.Views.ToList();
                                var userView = new List<UserView>
                                {
                                    new UserView { UserId=newUser.UserId,ViewId=allViews.First(v=>v.Title == "Dashboard").ViewId},
                                    new UserView { UserId=newUser.UserId,ViewId=allViews.First(v=>v.Title == "Admin").ViewId},
                                };
                                dbContext.UserViews.AddRange(userView);
                                dbContext.SaveChanges();
                                return new FranchiseUserDto
                                {
                                    FranchiseId = newFranchise.FranchiseId,
                                    FranchiseName = newFranchise.Name,
                                    FranchiseUser = Mapper.Map<SimpleUserDto>(newUser)
                                };
                            }
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

        public FranchiseUserDto UpdateFranchise(int franchiseId,FranchiseUserDto franchiseDto)
        {
            using (var dbContext = new OmContext())
            {
                try
                {
                    var franchise = dbContext.Franchises.FirstOrDefault(f => f.FranchiseId == franchiseId);
                    if (franchise == null)
                    {
                        return null;
                    }
                    franchise.Name = franchiseDto.FranchiseName;
                    var franchiseUser = dbContext.Users.FirstOrDefault(fu => fu.UserId == franchise.UserId);
                    if (franchiseUser != null)
                    {
                        franchiseUser.Name = franchiseDto.FranchiseUser.Name;
                        franchiseUser.Email = franchiseDto.FranchiseUser.Email;
                        franchiseUser.Password = franchiseDto.FranchiseUser.Password;
                        franchiseUser.cnic = franchiseDto.FranchiseUser.cnic;
                    };
                    dbContext.SaveChanges();
                    return new FranchiseUserDto
                    {
                        FranchiseId = franchiseId,
                        FranchiseName = franchise.Name,
                        FranchiseUser = Mapper.Map<SimpleUserDto>(franchiseUser)
                    };
                }
                catch (Exception e)
                {
                    return null;
                }
            }
        }
    }
}
