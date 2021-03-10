using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Data.Config;
using Data.Models;
using Repository.DTOModel;

namespace Repository.Repository
{
    public class FranchiseAdminRepository
    {
        private readonly UserRepository _userRepository;

        public FranchiseAdminRepository()
        {
            _userRepository = new UserRepository();
        }

        public SimpleUserDto AddFranchiseAdmin(SimpleUserDto franchiseAdminDto,int userId)
        {
            using (var dbContext = new OmContext())
            {
                try
                {
                    var franchise = dbContext.Franchises.FirstOrDefault(f => f.UserId == userId);
                    if (franchise != null)
                    {
                        var user = dbContext.Users.FirstOrDefault(a => a.Email == franchiseAdminDto.Email);
                        if (user == null)
                        {
                            User newUser = new User()
                            {
                                Name = franchiseAdminDto.Name,
                                Email = franchiseAdminDto.Email,
                                Password = franchiseAdminDto.Password,
                                cnic = franchiseAdminDto.cnic,
                            };
                            dbContext.Users.Add(newUser);
                            var role = dbContext.Roles.FirstOrDefault(r => r.Title == "Admin");
                            if (role != null)
                            {
                                var userRole = new UserRole
                                {
                                    UserId = newUser.UserId,
                                    RoleId = role.RoleId
                                };
                                dbContext.UserRoles.Add(userRole);
                                dbContext.FranchiseAdmins.Add(new FranchiseAdmin
                                {
                                    FranchiseId = franchise.FranchiseId,
                                    UserId = newUser.UserId
                                });
                                var allViews = dbContext.Views.ToList();
                                var userView = new List<UserView>
                                {
                                    new UserView { UserId=newUser.UserId,ViewId=allViews.First(v=>v.Title == "Dashboard").ViewId},
                                    new UserView { UserId=newUser.UserId,ViewId=allViews.First(v=>v.Title == "Customer").ViewId},
                                    new UserView { UserId=newUser.UserId,ViewId=allViews.First(v=>v.Title == "Worker").ViewId},
                                    new UserView { UserId=newUser.UserId,ViewId=allViews.First(v=>v.Title == "Category").ViewId},
                                    new UserView { UserId=newUser.UserId,ViewId=allViews.First(v=>v.Title == "Services").ViewId},
                                    new UserView { UserId=newUser.UserId,ViewId=allViews.First(v=>v.Title == "Jobs").ViewId},
                                };
                                dbContext.UserViews.AddRange(userView);
                                dbContext.SaveChanges();
                                return Mapper.Map<SimpleUserDto>(newUser);
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
        public SimpleUserDto UpdateFranchiseAdmin(SimpleUserDto franchiseAdminDto)
        {
            using (var dbContext = new OmContext())
            {
                try
                {
                    var user = dbContext.Users.FirstOrDefault(a => a.Email == franchiseAdminDto.Email);
                    if (user != null)
                    {
                        user.Name = franchiseAdminDto.Name;
                        user.Email = franchiseAdminDto.Email;
                        user.Password = franchiseAdminDto.Password;
                        user.cnic = franchiseAdminDto.cnic;
                    }
                        if (user == null)
                        {
                            User newUser = new User()
                            {
                                Name = franchiseAdminDto.Name,
                                Email = franchiseAdminDto.Email,
                                Password = franchiseAdminDto.Password,
                                cnic = franchiseAdminDto.cnic,
                            };
                            
                        }
                    dbContext.SaveChanges();

                    return Mapper.Map<SimpleUserDto>(user);
                }
                catch (Exception e)
                {
                    return null;
                }
            }
        }
       
        public List<SimpleUserDto> GetAllFranchisesAdmin(int userId)
        {
            using (var dbContext = new OmContext())
            {
                var franchisesAdmins = (from fa in dbContext.FranchiseAdmins
                                  join f in dbContext.Franchises on fa.FranchiseId equals f.FranchiseId
                                  join u in dbContext.Users on fa.UserId equals u.UserId
                                  where f.UserId == userId
                                        select new SimpleUserDto
                                   {
                                          UserId = u.UserId,
                                          Name = u.Name,
                                          Email = u.Email,
                                          Password = u.Password,
                                          cnic = u.cnic
                                    
                                  }).ToList();
                return franchisesAdmins;
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
                                  where r.Title == "Franchise" && f.FranchiseId == franchiseId
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
        public SimpleUserDto GetFranchiseAdmin(int adminId)
        {
            using (var dbContext = new OmContext())
            {
                var franchises = (from f in dbContext.Franchises
                    join u in dbContext.Users on f.UserId equals u.UserId
                    join ur in dbContext.UserRoles on u.UserId equals ur.UserId
                    join r in dbContext.Roles on ur.RoleId equals r.RoleId
                    where r.Title == "Franchise" && f.UserId == adminId
                    select new SimpleUserDto
                    {
                            UserId = u.UserId,
                            Name = u.Name,
                            Email = u.Email,
                            Password = u.Password,
                            cnic = u.cnic
                    }).FirstOrDefault();
                return franchises;
            }
        }
      
        public FranchiseWorkerDto GetFranchiseCustomer(int customerId)
        {
            using (var dbContext = new OmContext())
            {
                var franchises = (from f in dbContext.Franchises
                    join u in dbContext.Users on f.UserId equals u.UserId
                    join ur in dbContext.UserRoles on u.UserId equals ur.UserId
                    join r in dbContext.Roles on ur.RoleId equals r.RoleId
                    where r.Title == "Franchise" && f.UserId == customerId
                    select new FranchiseWorkerDto
                    {
                        FranchiseId = f.FranchiseId,
                        FranchiseName = f.Name,
                        FranchiseWorker = new SimpleUserDto
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
        public bool DeleteFranchiseAdmin(int userId)
        {
            using (var dbContext = new OmContext())
            {
                var user = dbContext.Users.FirstOrDefault(u => u.UserId == userId);
                if (user != null)
                {

                    var franchiseAdmin = dbContext.FranchiseAdmins.FirstOrDefault(a => a.UserId == userId);
                    var roles= dbContext.UserRoles.Where(a => a.UserId == userId).ToList();
                    var views = dbContext.UserViews.Where(a => a.UserId == userId).ToList();
                    dbContext.UserRoles.RemoveRange(roles);
                    dbContext.UserViews.RemoveRange(views);
                    dbContext.Users.Remove(user);
                    dbContext.SaveChanges();
                    return true;
                }
                return false;
            }
        }  
    }
}