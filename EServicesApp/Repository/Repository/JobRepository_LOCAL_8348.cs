using System;
using System.Collections.Generic;
using System.Linq;
using Data.Config;
using Data.Models;
using Repository.DTOModel;
using System.Device.Location;
using Repository.Helper;

namespace HeadsUp.Repository.Repositories
{
    public class JobRepository
    {
        private readonly fcm fcm;
        public JobRepository()
        {
            fcm = new fcm();
        }
        public bool RequestJob(JobDto jobDto)
        {
            using (var dbContext = new OmContext())
            {
                try
                {
                    var service = dbContext.Services.FirstOrDefault(x => x.ServiceId == jobDto.ServiceId);

                    var customerLocation = (from c in dbContext.Customers
                                    join a in dbContext.Addresses on c.AddressId equals a.AddressId
                                    where c.CustomerId == jobDto.CustomerId
                                    select new AddressDto
                                    {
                                        Latitude =a.Latitude,
                                        Longitude = a.Longitude
                                    }).FirstOrDefault();

                    var nearsetWorker = closestWorker(jobDto.CustomerId,service.WorkCategoryId);
                    if (nearsetWorker != null) {
                        var worker = dbContext.Workers.FirstOrDefault(w => w.WorkerId == nearsetWorker.WorkerId);
                        worker.status = 1;

                        Location newLocation = new Location
                        {
                            Latitude = Double.Parse(customerLocation.Latitude),
                            Longitude = Double.Parse(customerLocation.Longitude)
                        };
                        dbContext.Locations.Add(newLocation);
                        
                        Job newJob = new Job
                        {
                            ServiceId = jobDto.ServiceId,
                            CustomerId = jobDto.CustomerId,
                            WorkerId = nearsetWorker.WorkerId,
                            RequestedOn = DateTime.Now,
                            LocationId=newLocation.LocationId,
                            status = 0 // 0 for Requested -1 for cancel //1 for completed
                        };

                        var workerUserId = dbContext.Workers.FirstOrDefault(a => a.WorkerId == nearsetWorker.WorkerId).UserId;
                        var customerUserId = dbContext.Customers.FirstOrDefault(a => a.CustomerId == jobDto.CustomerId).UserId;

                        var customerNotificationToken = dbContext.NotificationDetails.FirstOrDefault(a=>a.UserId == customerUserId).Token;

                        var workerNotificationToken = dbContext.NotificationDetails.FirstOrDefault(a => a.UserId == workerUserId).Token;

                        var pushNotification = new PushNotification
                        {
                            to = workerNotificationToken,
                            data = new PushNotificationPayLoad
                            {
                                ActivityId = 1,
                                title = "Job  has been Assigned to You",
                                body = "....................... ",
                                IsSoundEnabled = true,
                                NotificationData = new
                                {
                                    abc = 123,
                                    xyz = 154
                                }
                            }
                        };
                        fcm.sendNotification(pushNotification);

                        var newpushNotification = new PushNotification
                        {
                            to = customerNotificationToken,
                            data = new PushNotificationPayLoad
                            {
                                ActivityId = 1,
                                title = "Worker Have been Assigned against your Job",
                                body = "....................... ",
                                IsSoundEnabled = true,
                                NotificationData = new
                                {
                                    abc = 123,
                                    xyz = 154
                                }
                            }
                        };
                        fcm.sendNotification(newpushNotification);
                        dbContext.Jobs.Add(newJob);
                        dbContext.SaveChanges();
                        return true;
                    }
                    else
                    {
                        var customerUserId = dbContext.Customers.FirstOrDefault(a => a.CustomerId == jobDto.CustomerId).UserId;
                        var customerNotificationToken = dbContext.NotificationDetails.FirstOrDefault(a => a.UserId == customerUserId).Token;
                        var pushNotification = new PushNotification
                        {
                            to = customerNotificationToken,
                            data = new PushNotificationPayLoad
                            {
                                ActivityId = 1,
                                title = "No worker available nearby !",
                                body = "Please try again Later",
                                IsSoundEnabled = true,
                                NotificationData = new
                                {
                                    abc = 123,
                                    xyz = 154
                                }
                            }
                        };
                        fcm.sendNotification(pushNotification);
                        return false;
                    }
                    
                }
                catch (Exception e)
                {
                    return false;
                }
            }
        }

        public bool JobCancelled(int jobId)
        {
            using (var dbContext = new OmContext())
            {
                try
                {
                    var job = dbContext.Jobs.First(j => j.JobId == jobId);
                    job.status = -1; //-1 for jon canceled
                    dbContext.SaveChanges();
                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
        }

        public bool AddRating(int jobId,float rating)
        {
            using (var dbContext = new OmContext())
            {
                try
                {
                    var job = dbContext.Jobs.First(j => j.JobId == jobId);
                    job.Rate = rating;

                    var worker = dbContext.Workers.First(w => w.WorkerId == job.WorkerId);
                    if (worker.Rate == 0)
                        worker.Rate = rating;
                    else
                        worker.Rate = (worker.Rate + rating) / 2;

                    dbContext.SaveChanges();
                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
        }

        //Getting the list of Jobs of house Owner that has been completed
        public List<JobInfromationDto> getCompletedJobofCustomer(int customerId)
        {
            using (var dbContext = new OmContext())
            {
                try
                {
                    var jobDtos = (from jb in dbContext.Jobs
                        join lc in dbContext.Locations on jb.LocationId equals lc.LocationId
                        join sc in dbContext.Services on jb.ServiceId equals sc.ServiceId
                        join wc in dbContext.WorkCategory on sc.WorkCategoryId equals wc.WorkCategoryId
                        join wr in dbContext.Workers on jb.WorkerId equals wr.WorkerId
                        join us in dbContext.Users on wr.UserId equals us.UserId
                        join fn in dbContext.Franchises on wc.FranchiseId equals fn.FranchiseId
                        where jb.CustomerId == customerId && jb.status == 2
                        orderby jb.CompletedOn descending
                                   select new JobInfromationDto
                                   {
                                       JobDto = new JobDto
                                       {
                                           JobId = jb.JobId,
                                           CategoryName = wc.Title,
                                           CustomerId = jb.CustomerId,
                                           WorkerId = jb.WorkerId,
                                           WorkerUserId=wr.UserId,
                                           RequestedOn = jb.RequestedOn,
                                           CompletedOn= jb.CompletedOn ?? DateTime.Now,
                                           status = jb.status,
                                           ServiceId = sc.ServiceId,
                                           ServiceName = sc.Title,
                                           WorkerName = us.Name,
                                           Rate=jb.Rate
                                       },
                                       CustomerDetail = new SimpleUserDto
                                       {
                                           UserId = us.UserId,
                                           Name = us.Name,
                                           Email = us.Email,
                                           cnic = us.cnic,
                                           phone = us.phone
                                       },
                                       CustomerLocation = new LocationDto
                                       {
                                           Latitude = lc.Latitude,
                                           Longitude = lc.Longitude
                                       },
                                       ServiceDto= new ServiceDto
                                       {
                                           Title=sc.Title,
                                           Price=sc.Price
                                       }
                                   }).ToList();
                    return jobDtos;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }

        public List<JobInfromationDto> getCompletedJobofWorker(int workerId)
        {
            using (var dbContext = new OmContext())
            {
                try
                {
                    var jobDtos = (from jb in dbContext.Jobs
                                   join lc in dbContext.Locations on jb.LocationId equals lc.LocationId
                                   join sc in dbContext.Services on jb.ServiceId equals sc.ServiceId
                                   join wc in dbContext.WorkCategory on sc.WorkCategoryId equals wc.WorkCategoryId
                                   join wr in dbContext.Workers on jb.WorkerId equals wr.WorkerId
                                   join cu in dbContext.Customers on jb.CustomerId equals cu.CustomerId
                                   join us in dbContext.Users on cu.UserId equals us.UserId
                                   join a in dbContext.Addresses on cu.AddressId equals a.AddressId
                                   where jb.WorkerId == workerId && jb.status == 2
                                   orderby jb.CompletedOn descending
                                   select new JobInfromationDto
                                   {
                                       JobDto = new JobDto
                                       {
                                           JobId = jb.JobId,
                                           CategoryName = wc.Title,
                                           CustomerId = jb.CustomerId,
                                           WorkerId = jb.WorkerId,
                                           CompletedOn= jb.CompletedOn ?? DateTime.Now,
                                           RequestedOn = jb.RequestedOn,
                                           status = jb.status,
                                           ServiceId = sc.ServiceId,
                                           ServiceName = sc.Title,
                                           WorkerName = us.Name,
                                           Rate = jb.Rate
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
                                       CustomerLocation = new LocationDto
                                       {
                                           Latitude=lc.Latitude,
                                           Longitude=lc.Longitude
                                       },
                                       
                                       ServiceDto = new ServiceDto
                                       {
                                           Title = sc.Title,
                                           Price = sc.Price
                                       }
                                   }).ToList();
                    return jobDtos;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }

        //Getting the list of Jobs of house Owner that has been requested
        public List<JobInfromationDto> getPendingJobofCustomer(int customerId)
        {
            using (var dbContext = new OmContext())
            {
                try
                {
                    var jobDtos = (from jb in dbContext.Jobs
                        join lc in dbContext.Locations on jb.LocationId equals lc.LocationId
                        join sc in dbContext.Services on jb.ServiceId equals sc.ServiceId
                        join wc in dbContext.WorkCategory on sc.WorkCategoryId equals wc.WorkCategoryId
                        join fn in dbContext.Franchises on wc.FranchiseId equals fn.FranchiseId
                        join wr in dbContext.Workers on jb.WorkerId equals wr.WorkerId
                                   join us in dbContext.Users on wr.UserId equals us.UserId
                        where jb.CustomerId == customerId &&( jb.status == 0 || jb.status==3 || jb.status==1)
                                   orderby jb.RequestedOn descending
                                   select new JobInfromationDto
                                   {
                                       JobDto = new JobDto
                                       {
                                           JobId = jb.JobId,
                                           CategoryName = wc.Title,
                                           CustomerId = jb.CustomerId,
                                           WorkerId = jb.WorkerId,
                                           WorkerUserId=wr.UserId,
                                           RequestedOn = jb.RequestedOn,
                                           status = jb.status,
                                           ServiceId = sc.ServiceId,
                                           ServiceName = sc.Title,
                                           WorkerName = us.Name,
                                           Rate = jb.Rate
                                       },
                                       CustomerLocation = new LocationDto
                                       {
                                           Latitude = lc.Latitude,
                                           Longitude = lc.Longitude
                                       },
                                       CustomerDetail = new SimpleUserDto
                                       {
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
                catch (Exception ex)
                {
                    return null;
                }
            }
        }

   

        public List<JobInfromationDto> getPendingJobofWorker(int workerId)
        {
            using (var dbContext = new OmContext())
            {
                try
                {
                    var jobDtos = (from jb in dbContext.Jobs
                                   join sc in dbContext.Services on jb.ServiceId equals sc.ServiceId
                                   join wc in dbContext.WorkCategory on sc.WorkCategoryId equals wc.WorkCategoryId
                                   join wr in dbContext.Workers on jb.WorkerId equals wr.WorkerId
                                   join cu in dbContext.Customers on jb.CustomerId equals cu.CustomerId
                                   join us in dbContext.Users on cu.UserId equals us.UserId
                                   join a in dbContext.Addresses on cu.AddressId equals a.AddressId
                                   where jb.WorkerId == workerId && jb.status != 2
                                   orderby jb.RequestedOn descending
                                   select new JobInfromationDto
                                   {
                                       JobDto= new JobDto
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
                                       CustomerDetail=new SimpleUserDto
                                       {
                                           UserId=us.UserId,
                                           Name=us.Name,
                                           Email=us.Email,
                                           cnic=us.cnic,
                                           phone=us.phone
                                       },
                                       ServiceDto = new ServiceDto
                                       {
                                           Title = sc.Title,
                                           Price = sc.Price
                                       }
                                   }).ToList();
                    return jobDtos;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }

        public bool StartJob(int jobId)
        {
            using (var dbContext = new OmContext())
            {
                var job = dbContext.Jobs.FirstOrDefault(j => j.JobId == jobId);
                var service = dbContext.Services.FirstOrDefault(s => s.ServiceId == job.ServiceId);
               
                    var newJobDetail = new JobDetail
                    {
                        JobId = jobId,
                        status = 0,
                        StartedOn = DateTime.Now,
                        FinishedOn = null
                    };
                    dbContext.JobDetails.Add(newJobDetail);
                
                job.status = 1;
                job.Visit++;

                var workerUserId = dbContext.Workers.FirstOrDefault(a => a.WorkerId == job.WorkerId).UserId;
                var customerUserId = dbContext.Customers.FirstOrDefault(a => a.CustomerId == job.CustomerId).UserId;

                var customerNotificationToken = dbContext.NotificationDetails.FirstOrDefault(a => a.UserId == customerUserId).Token;

                var workerNotificationToken = dbContext.NotificationDetails.FirstOrDefault(a => a.UserId == workerUserId).Token;

                var pushNotification = new PushNotification
                {
                    to = workerNotificationToken,
                    data = new PushNotificationPayLoad
                    {
                        ActivityId = 1,
                        title = "You have started Job",
                        body = service.Title,
                        IsSoundEnabled = true,
                        NotificationData = new
                        {
                            abc = 123,
                            xyz = 154
                        }
                    }
                };
                fcm.sendNotification(pushNotification);

                var newpushNotification = new PushNotification
                {
                    to = customerNotificationToken,
                    data = new PushNotificationPayLoad
                    {
                        ActivityId = 1,
                        title = "Your requested jobs has been requested",
                        body = service.Title,
                        IsSoundEnabled = true,
                        NotificationData = new
                        {
                            abc = 123,
                            xyz = 154
                        }
                    }
                };
                fcm.sendNotification(newpushNotification);

                dbContext.SaveChanges();

                
                return true;
            }
        }
        public bool RescheduleJob(int jobId,String date)
        {
            using (var dbContext = new OmContext())
            {
                var job = dbContext.Jobs.FirstOrDefault(j => j.JobId == jobId);
                job.status = 3;

                var worker = dbContext.Workers.FirstOrDefault(w => w.WorkerId == job.WorkerId);
                worker.status = 0;


                var jobDetail = dbContext.JobDetails.Where(j => j.JobId == jobId)
                     .OrderByDescending(q => q.JobDetailId).FirstOrDefault();
                jobDetail.status = 3; //Rescheduled
                jobDetail.FinishedOn = Convert.ToDateTime(date);
                dbContext.SaveChanges();

                var service = dbContext.Services.FirstOrDefault(s => s.ServiceId == job.ServiceId);

                var workerUserId = dbContext.Workers.FirstOrDefault(a => a.WorkerId == job.WorkerId).UserId;
                var customerUserId = dbContext.Customers.FirstOrDefault(a => a.CustomerId == job.CustomerId).UserId;

                var customerNotificationToken = dbContext.NotificationDetails.FirstOrDefault(a => a.UserId == customerUserId).Token;

                var workerNotificationToken = dbContext.NotificationDetails.FirstOrDefault(a => a.UserId == workerUserId).Token;

                var pushNotification = new PushNotification
                {
                    to = workerNotificationToken,
                    data = new PushNotificationPayLoad
                    {
                        ActivityId = 1,
                        title = "You have rescheduled Job",
                        IsSoundEnabled = true,
                        NotificationData = new
                        {
                            abc = 123,
                            xyz = 154
                        }
                    }
                };
                fcm.sendNotification(pushNotification);

                var newpushNotification = new PushNotification
                {
                    to = customerNotificationToken,
                    data = new PushNotificationPayLoad
                    {
                        ActivityId = 1,
                        title = "Job Rescheduled ",
                        body = "Your requested job has been rescheduled",
                        IsSoundEnabled = true,
                        NotificationData = new
                        {
                            abc = 123,
                            xyz = 154
                        }
                    }
                };
                fcm.sendNotification(newpushNotification);

                return true;
            }
        }
        public bool StopJob(int jobId)
        {
            using (var dbContext = new OmContext())
            {
                var job = dbContext.Jobs.FirstOrDefault(j => j.JobId == jobId);
                job.status = 2; //Completed
                job.CompletedOn = DateTime.Now;

                var worker = dbContext.Workers.FirstOrDefault(w => w.WorkerId == job.WorkerId);
                worker.status = 0;
                var jobDetail = dbContext.JobDetails.Where(j => j.JobId == jobId)
                    .OrderByDescending(q => q.JobDetailId).FirstOrDefault();
                jobDetail.status = 2; //Completed
                jobDetail.FinishedOn = DateTime.Now;
                dbContext.SaveChanges();

                var service = dbContext.Services.FirstOrDefault(s => s.ServiceId == job.ServiceId);

                var workerUserId = dbContext.Workers.FirstOrDefault(a => a.WorkerId == job.WorkerId).UserId;
                var customerUserId = dbContext.Customers.FirstOrDefault(a => a.CustomerId == job.CustomerId).UserId;

                var customerNotificationToken = dbContext.NotificationDetails.FirstOrDefault(a => a.UserId == customerUserId).Token;

                var workerNotificationToken = dbContext.NotificationDetails.FirstOrDefault(a => a.UserId == workerUserId).Token;

                var pushNotification = new PushNotification
                {
                    to = workerNotificationToken,
                    data = new PushNotificationPayLoad
                    {
                        ActivityId = 1,
                        title = "You have completed the Job",
                        IsSoundEnabled = true,
                        NotificationData = new
                        {
                            abc = 123,
                            xyz = 154
                        }
                    }
                };
                fcm.sendNotification(pushNotification);

                var newpushNotification = new PushNotification
                {
                    to = customerNotificationToken,
                    data = new PushNotificationPayLoad
                    {
                        ActivityId = 1,
                        title = "Job Completed",
                        body = "Your requested job has been Completed",
                        IsSoundEnabled = true,
                        NotificationData = new
                        {
                            abc = 123,
                            xyz = 154
                        }
                    }
                };
                fcm.sendNotification(newpushNotification);
                return true;
            }
        }

        public NearsetWorker closestWorker(int customerId, int workCategoryId)
        {
            using (var dbContext = new OmContext())
            {

                var customer = dbContext.Customers.FirstOrDefault(u => u.CustomerId == 2);
                var address = dbContext.Addresses.FirstOrDefault(a => a.AddressId == customer.AddressId);

                var nearesttworker=  dbContext.Workers
                    .Where(x=>x.status !=-1 && x.status!=1 && x.WorkCategoryId==workCategoryId)
                    .AsEnumerable()
                       .Select(x => new NearsetWorker
                       {
                           WorkerId = x.WorkerId,
                           distance = getDistance(Double.Parse(address.Latitude), Double.Parse(address.Longitude), Double.Parse(x.Latitude), Double.Parse(x.Longitude), 'K'),
                           Rate=x.Rate

                       })
                       .OrderBy(x => x.distance).ThenBy(x=>x.Rate).FirstOrDefault();

                return nearesttworker; 

            }
        }

        private double getDistance(double lat1, double lon1, double lat2, double lon2, char unit)
        {
            double theta = lon1 - lon2;
            double dist = Math.Sin(deg2rad(lat1)) * Math.Sin(deg2rad(lat2)) + Math.Cos(deg2rad(lat1)) * Math.Cos(deg2rad(lat2)) * Math.Cos(deg2rad(theta));
            dist = Math.Acos(dist);
            dist = rad2deg(dist);
            dist = dist * 60 * 1.1515;
            if (unit == 'K')
            {
                dist = dist * 1.609344;
            }
            else if (unit == 'N')
            {
                dist = dist * 0.8684;
            }
            return (dist);
        }

        /*:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::*/
        /*::  This function converts decimal degrees to radians             :*/
        /*:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::*/
        private double deg2rad(double deg)
        {
            return (deg * Math.PI / 180.0);
        }

        /*:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::*/
        /*::  This function converts radians to decimal degrees             :*/
        /*:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::*/
        private double rad2deg(double rad)
        {
            return (rad * 180.0 / Math.PI);
        }
        public AllJobsDto GetAllJobsofFranchise(int userId)
        {
            using (var dbContext = new OmContext())
            {
                try
                {
                    int franchiseId;
                    var franchise = dbContext.Franchises.FirstOrDefault(a => a.UserId == userId);
                    if(franchise == null)
                    {
                        franchiseId = dbContext.FranchiseAdmins.FirstOrDefault(a => a.UserId == userId).FranchiseId;
                    }else
                    {
                        franchiseId = franchise.FranchiseId;
                    }


                    var jobs = (from jb in dbContext.Jobs
                                   join lc in dbContext.Locations on jb.LocationId equals lc.LocationId
                                   join sc in dbContext.Services on jb.ServiceId equals sc.ServiceId
                                   join wc in dbContext.WorkCategory on sc.WorkCategoryId equals wc.WorkCategoryId
                                   join wr in dbContext.Workers on jb.WorkerId equals wr.WorkerId
                                   join cu in dbContext.Customers on jb.CustomerId equals cu.CustomerId
                                   join us in dbContext.Users on cu.UserId equals us.UserId
                                   join a in dbContext.Addresses on cu.AddressId equals a.AddressId
                                   where wr.FranchiseId == franchiseId 
                                   orderby jb.RequestedOn descending
                                   select new AllJobDetailDto
                                   {
                                       JobId=jb.JobId,
                                       RequestedBy = us.Name,
                                       AssignedTo="",
                                       Charges=jb.Rate,
                                       RequestedOn=jb.RequestedOn,
                                       CompletedOn=jb.CompletedOn,
                                       RequestedService=sc.Title,
                                       RescheduledOn=jb.CompletedOn,
                                       StartedOn=null,
                                       Status=jb.status
                                   }).ToList();
                    var allJobs = new AllJobsDto
                    {
                        PendingJobs = (from j in jobs
                                       where j.Status == 0
                                       select new PendingJobDetailDto
                                       {
                                           JobId = j.JobId,
                                           RequestedBy = j.RequestedBy,
                                           AssignedTo = j.AssignedTo,
                                           RequestedOn = j.RequestedOn,
                                           RequestedService = j.RequestedService,
                                           Charges = j.Charges
                                       }).ToList(),
                        InProgressJobs = (from j in jobs
                                          where j.Status == 1
                                          select new InProgressJobDetailDto
                                          {
                                              JobId = j.JobId,
                                              RequestedBy = j.RequestedBy,
                                              AssignedTo = j.AssignedTo,
                                              RequestedOn = j.RequestedOn,
                                              RequestedService = j.RequestedService,
                                              Charges = j.Charges,
                                              StartedOn = j.StartedOn
                                          }).ToList(),
                        CompletedJobs = (from j in jobs
                                         where j.Status == 2
                                         select new CompletedJobDetailDto
                                         {
                                             JobId = j.JobId,
                                             RequestedBy = j.RequestedBy,
                                             AssignedTo = j.AssignedTo,
                                             RequestedOn = j.RequestedOn,
                                             RequestedService = j.RequestedService,
                                             Charges = j.Charges,
                                             StartedOn = j.StartedOn,
                                             CompletedOn = j.CompletedOn
                                         }).ToList(),
                        RescheduledJob = (from j in jobs
                                          where j.Status == 3
                                          select new RescheduledJobDetailDto
                                          {
                                              JobId = j.JobId,
                                              RequestedBy = j.RequestedBy,
                                              AssignedTo = j.AssignedTo,
                                              RequestedOn = j.RequestedOn,
                                              RequestedService = j.RequestedService,
                                              Charges = j.Charges,
                                              StartedOn = j.StartedOn,
                                              RescheduledOn = j.RescheduledOn
                                          }).ToList()
                    };
                    return allJobs;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }
    }
}