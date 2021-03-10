using System;
using System.Collections.Generic;
using System.Linq;
using Data.Config;
using Data.Models;
using Repository.DTOModel;

namespace HeadsUp.Repository.Repositories
{
    public class ServiceRepository
    {
        public ServiceDto AddService(ServiceDto serviceDto)
        {
            using (var dbContext = new OmContext())
            {
                try
                {
                   // var serviceCategory = dbContext.Services.Where(w => w.Title == serviceDto.Title);
                   // if (!serviceCategory.Any())
                    {
                        Service newService = new Service
                        {
                            WorkCategoryId = serviceDto.WorkCategoryId,
                            Title = serviceDto.Title,
                            Price = serviceDto.Price
                        };

                        dbContext.Services.Add(newService);
                        dbContext.SaveChanges();
                        return new ServiceDto() {
                            Title=newService.Title,
                            Price=newService.Price,
                            WorkCategoryId=newService.WorkCategoryId,
                            ServiceId=newService.ServiceId
                        };
                    }

                    return null;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }

        public List<ServiceDto> GetAllServices(int workCategoryId)
        {
            using (var dbContext = new OmContext())
            {
                try
                {
                    var services = dbContext.Services.Where(s => s.WorkCategoryId == workCategoryId).Select(s=>new ServiceDto() {
                        WorkCategoryId=s.WorkCategoryId,
                        Title=s.Title,
                        Price=s.Price,
                        ServiceId=s.ServiceId
                    }).ToList();

                    return services;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }
        public ServiceDto GetServiceDetail(int serviceId)
        {
            using (var dbContext = new OmContext())
            {
                try
                {
                    var service = dbContext.Services.FirstOrDefault(s => s.ServiceId == serviceId);
                    return new ServiceDto()
                    {
                        WorkCategoryId = service.WorkCategoryId,
                        Title = service.Title,
                        Price = service.Price,
                        ServiceId = service.ServiceId
                    };
                    
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }

        public ServiceDto UpdateService(ServiceDto serviceDto)
        {
            using (var dbContext = new OmContext())
            {
                try
                {
                    var service = dbContext.Services.FirstOrDefault(s => s.ServiceId == serviceDto.ServiceId);
                    if (service != null)
                    {
                        service.Title = serviceDto.Title;
                        service.Price = serviceDto.Price;
                        dbContext.SaveChanges();
                        return new ServiceDto()
                        {
                            ServiceId=service.ServiceId,
                            Title = service.Title,
                            Price = service.Price,
                            WorkCategoryId = service.WorkCategoryId
                        };
                    }

                    return null;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }

        public bool DeleteService(int serviceId)
        {
            using (var dbContext = new OmContext())
            {
                try
                {
                    var service = dbContext.Services.FirstOrDefault(u => u.ServiceId == serviceId);
                if (service != null)
                {
                    dbContext.Services.Remove(service);
                    dbContext.SaveChanges();
                    return true;
                }
                }
                catch (Exception e)
                {
                    return false;
                }
                return false;
            }
        }
    }
}