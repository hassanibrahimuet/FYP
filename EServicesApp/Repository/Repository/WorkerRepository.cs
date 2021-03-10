using System;
using System.Collections.Generic;
using System.Linq;
using Data.Config;
using Data.Models;
using Repository.DTOModel;
using Repository.Repository;
using AutoMapper;

namespace HeadsUp.Repository.Repositories
{
    public class WorkerRepository
    {
        private readonly UserRepository _userRepository;

        public WorkerRepository()
        {
            _userRepository = new UserRepository();
        }

        public WorkerUserDto AddFranchiseWorker(WorkerUserDto userDto,int userId)
        {
            using (var dbContext = new OmContext())
            {
                try
                {

                    var franchise = (from f in dbContext.Franchises
                                    join fa in dbContext.FranchiseAdmins on f.FranchiseId equals fa.FranchiseId
                                    where fa.UserId == userId
                                    select f).FirstOrDefault();
                    if (franchise != null)
                    {
                        var user = dbContext.Users.FirstOrDefault(a => a.Email == userDto.Email);
                        if (user == null)
                        {
                            User newUser = new User()
                            {
                                Name = userDto.Name,
                                Email = userDto.Email,
                                Password = userDto.Password,
                                cnic = userDto.cnic,
                                phone=userDto.phone
                            };
                            dbContext.Users.Add(newUser);
                            var role = dbContext.Roles.FirstOrDefault(r => r.Title == "Worker");
                            if (role != null)
                            {
                                var userRole = new UserRole
                                {
                                    UserId = newUser.UserId,
                                    RoleId = role.RoleId
                                };
                                dbContext.UserRoles.Add(userRole);
                                dbContext.SaveChanges();
                                Worker newWorker = new Worker()
                                {
                                    FranchiseId=franchise.FranchiseId,
                                    UserId=newUser.UserId,
                                    WorkCategoryId= userDto.WorkCategoryId,
                                    status=-1
                                    
                                };
                                dbContext.Workers.Add(newWorker);
                                dbContext.SaveChanges();
                                return new WorkerUserDto
                                {
                                    UserId=newUser.UserId,
                                    Name=newUser.Name,
                                    Email=newUser.Email,
                                    Password=newUser.Password,
                                    cnic=newUser.cnic,
                                    WorkCategoryId= userDto.WorkCategoryId,
                                    phone=userDto.phone
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

        public List<WorkerUserDto> GetAllFranchisesWorkers(int userId)
        {
            using (var dbContext = new OmContext())
            {
                var franchisesWorkers = (from fw in dbContext.Workers
                                        join f in dbContext.Franchises on fw.FranchiseId equals f.FranchiseId
                                         join fa in dbContext.FranchiseAdmins on f.FranchiseId equals fa.FranchiseId
                                        join u in dbContext.Users on fw.UserId equals u.UserId
                                        where fa.UserId == userId
                                        select new WorkerUserDto
                                        {
                                            UserId = u.UserId,
                                            Name = u.Name,
                                            Email = u.Email,
                                            Password = u.Password,
                                            cnic = u.cnic,
                                            WorkCategoryId = fw.WorkCategoryId,
                                            phone=u.phone
                                        }).ToList();
                return franchisesWorkers;
            }
        }
        public FranchiseWorkerDto GetFranchiseWorker(int userId)
        {
            using (var dbContext = new OmContext())
            {
                return (from u in dbContext.Users
                                  join w in dbContext.Workers on u.UserId equals w.UserId
                                  where u.UserId == userId
                                  select new FranchiseWorkerDto
                                  {
                                      FranchiseId = w.FranchiseId,
                                      WorkCategoryId=w.WorkCategoryId,
                                      WorkerId=w.WorkerId,
                                      FranchiseWorker = new SimpleUserDto
                                      {
                                          UserId = u.UserId,
                                          Name = u.Name,
                                          Email = u.Email,
                                          Password = u.Password,
                                          cnic = u.cnic,
                                          phone=u.phone
                                      }
                                  }).FirstOrDefault();
            }
        }
        public bool DeleteFranchiseWorker(int userId)
        {
            using (var dbContext = new OmContext())
            {
                try
                {
                    var user = dbContext.Users.FirstOrDefault(u => u.UserId == userId);
                    if (user != null)
                    {
                        var worker = dbContext.Workers.FirstOrDefault(u => u.UserId == user.UserId);
                        if (worker != null)
                        {
                            var userrole = dbContext.UserRoles.FirstOrDefault(a => a.UserId == user.UserId);
                            if (userrole != null)
                                dbContext.UserRoles.Remove(userrole);
                            dbContext.Workers.Remove(worker);
                            dbContext.Users.Remove(user);
                            dbContext.SaveChanges();
                            return true;
                        }
                        return false;
                    }
                    return false;
                }
                catch (Exception e)
                {
                    return false;
                }
            }
        }
        public WorkerUserDto UpdateFranchiseWorker(WorkerUserDto franchiseAdminDto)
        {
            using (var dbContext = new OmContext())
            {
                try
                {
                    var user = dbContext.Users.FirstOrDefault(a => a.Email == franchiseAdminDto.Email);
                    if (user != null)
                    {
                        var worker = dbContext.Workers.FirstOrDefault(a => a.UserId == franchiseAdminDto.UserId);
                        if (worker != null)
                            worker.WorkCategoryId = franchiseAdminDto.WorkCategoryId;
                        user.Name = franchiseAdminDto.Name;
                        user.Email = franchiseAdminDto.Email;
                        user.Password = franchiseAdminDto.Password;
                        user.cnic = franchiseAdminDto.cnic;
                        user.phone = franchiseAdminDto.phone;
                        dbContext.SaveChanges();
                        return new WorkerUserDto
                        {
                            UserId = user.UserId,
                            Name = user.Name,
                            Email = user.Email,
                            Password = user.Password,
                            cnic = user.cnic,
                            WorkCategoryId = worker.WorkCategoryId,
                            phone=user.phone
                        };
                    }
                    return franchiseAdminDto;
                }
                catch (Exception e)
                {
                    return null;
                }
            }
        }

        public bool UpdateWorkerLocation(int userId,string latitude,string longitude)
        {
            using (var dbContext = new OmContext())
            {
                var worker = dbContext.Workers.FirstOrDefault(u => u.UserId == userId);
                if (worker != null)
                {
                    worker.Latitude = latitude;
                    worker.Longitude = longitude;
                    dbContext.SaveChanges();
                    return true;
                }
                return false;
            }
        }
        public bool UpdateWorkerStatus(int workerId, int status)
        {
            using (var dbContext = new OmContext())
            {
                var worker = dbContext.Workers.FirstOrDefault(u => u.WorkerId == workerId);
                if (worker != null)
                {
                    worker.status = status;
                    dbContext.SaveChanges();
                    return true;
                }
                return false;
            }
        }
        public int getWorkerStatus(int workerId)
        {
            using (var dbContext = new OmContext())
            {
                var worker = dbContext.Workers.FirstOrDefault(u => u.WorkerId == workerId);
                if (worker != null)
                {
                    
                    return worker.status;
                }
                return -11;
            }
        }

    }
}