using Data.Config;
using Repository.DTOModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repository
{
    public class DashboardRepository
    {

        public SuperAdminDetialDto SuperAdminDashboard()
        {
            using (var dbContext = new OmContext())
            {

                var superadmins = (from  u in dbContext.Users
                                    join ur in dbContext.UserRoles on u.UserId equals ur.UserId
                                    join r in dbContext.Roles on ur.RoleId equals r.RoleId
                                    where r.Title == "SuperAdmin"
                                    select u).Count();

                var noOfWorkers = dbContext.Workers.Count();

                var noOfCustomers = dbContext.Customers.Count();

                var noOfFranchises = dbContext.Franchises.Count();


                return new SuperAdminDetialDto
                {
                    SuperAdmins = superadmins,
                    Workers = noOfWorkers,
                    Customers = noOfCustomers,
                    Franchises= noOfFranchises
                };
            }
        }

        public FranchiseDetailDto FranchiseDashboard(int userId)
        {
            using (var dbContext = new OmContext())
            {
                var franchiseId = (from fa in dbContext.FranchiseAdmins
                                 where fa.UserId == userId
                                 select fa.FranchiseId).FirstOrDefault();


                var noOfAdmins = dbContext.FranchiseAdmins.Where(f => f.FranchiseId == franchiseId).Count();

                var noOfWorkers = dbContext.Workers.Where(f => f.FranchiseId == franchiseId).Count();

                var noOfCustomers = dbContext.Customers.Where(f => f.FranchiseId == franchiseId).Count();

                var noOfCategories = dbContext.WorkCategory.Where(f => f.FranchiseId == franchiseId).Count();

                var noOfServicers = (from fa in dbContext.Services
                                     join w in dbContext.WorkCategory on fa.WorkCategoryId equals w.WorkCategoryId
                                     where w.FranchiseId == franchiseId
                                     select fa).Count();


                return new FranchiseDetailDto
                {
                    Admins = noOfAdmins,
                    Workers = noOfWorkers,
                    Customers = noOfCustomers,
                    Categories= noOfCategories,
                    Services= noOfServicers
                };
            }
        }
        public List<JobInfromationDto> FranchiseCompletedJobs(int userId)
        {
            using (var dbContext = new OmContext())
            {

                var franchise = dbContext.Franchises.FirstOrDefault(f => f.UserId == userId);
                var jobDtos = (from jb in dbContext.Jobs
                               join sc in dbContext.Services on jb.ServiceId equals sc.ServiceId
                               join wc in dbContext.WorkCategory on sc.WorkCategoryId equals wc.WorkCategoryId
                               join wr in dbContext.Workers on jb.WorkerId equals wr.WorkerId
                               join cu in dbContext.Customers on jb.CustomerId equals cu.CustomerId
                               join us in dbContext.Users on cu.UserId equals us.UserId
                               join a in dbContext.Addresses on cu.AddressId equals a.AddressId
                               where cu.FranchiseId == franchise.FranchiseId && jb.status != 2
                               orderby jb.RequestedOn descending
                               select new JobInfromationDto
                               {
                                   JobDto = new JobDto
                                   {
                                       JobId = jb.JobId,
                                       CategoryName = wc.Title,
                                       CustomerId = jb.CustomerId,
                                       WorkerId = jb.WorkerId,
                                       RequestedOn = jb.RequestedOn,
                                       status = jb.status,
                                       ServiceId = sc.ServiceId,
                                       ServiceName = sc.Title,
                                       WorkerName = us.Name,
                                       Visit = jb.Visit
                                   },
                                   AddressDto = new AddressDto
                                   {
                                       Country = a.Country,
                                       State = a.State,
                                       Street = a.Street,
                                       House = a.House,
                                       Latitude = a.Latitude,
                                       Longitude = a.Longitude
                                   },
                                   CustomerDetail = new SimpleUserDto
                                   {
                                       UserId = us.UserId,
                                       Name = us.Name,
                                       Email = us.Email,
                                       cnic = us.cnic,
                                       phone = us.phone
                                   },
                                   ServiceDto = new ServiceDto
                                   {
                                       Title = sc.Title,
                                       Price = sc.Price
                                   }
                               }).ToList();

                return jobDtos;
            }
        }
             public List<JobInfromationDto> FranchisePendingJobs(int userId)
        {
            using (var dbContext = new OmContext())
            {

                var franchise = dbContext.Franchises.FirstOrDefault(f => f.UserId == userId);
                var jobDtos = (from jb in dbContext.Jobs
                               join sc in dbContext.Services on jb.ServiceId equals sc.ServiceId
                               join wc in dbContext.WorkCategory on sc.WorkCategoryId equals wc.WorkCategoryId
                               join wr in dbContext.Workers on jb.WorkerId equals wr.WorkerId
                               join cu in dbContext.Customers on jb.CustomerId equals cu.CustomerId
                               join us in dbContext.Users on cu.UserId equals us.UserId
                               join a in dbContext.Addresses on cu.AddressId equals a.AddressId
                               where cu.FranchiseId == franchise.FranchiseId && (jb.status == 0 || jb.status == 3 || jb.status == 1)
                               orderby jb.RequestedOn descending
                               select new JobInfromationDto
                               {
                                   JobDto = new JobDto
                                   {
                                       JobId = jb.JobId,
                                       CategoryName = wc.Title,
                                       CustomerId = jb.CustomerId,
                                       WorkerId = jb.WorkerId,
                                       RequestedOn = jb.RequestedOn,
                                       status = jb.status,
                                       ServiceId = sc.ServiceId,
                                       ServiceName = sc.Title,
                                       WorkerName = us.Name,
                                       Visit = jb.Visit
                                   },
                                   AddressDto = new AddressDto
                                   {
                                       Country = a.Country,
                                       State = a.State,
                                       Street = a.Street,
                                       House = a.House,
                                       Latitude = a.Latitude,
                                       Longitude = a.Longitude
                                   },
                                   CustomerDetail = new SimpleUserDto
                                   {
                                       UserId = us.UserId,
                                       Name = us.Name,
                                       Email = us.Email,
                                       cnic = us.cnic,
                                       phone = us.phone
                                   },
                                   ServiceDto = new ServiceDto
                                   {
                                       Title = sc.Title,
                                       Price = sc.Price
                                   }
                               }).ToList();

                return jobDtos;
            }
        }
    }
}


