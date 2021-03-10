using System;
using System.Collections.Generic;
using System.Linq;
using Data.Config;
using Data.Models;
using Repository.DTOModel;

namespace HeadsUp.Repository.Repositories
{
    public class WorkCategoryRepository
    {
        public WorkCategoryDto AddWorkCategory(WorkCategoryDto workCategoryDto,int userId)
        {
            using (var dbContext = new OmContext())
            {
                try
                {
                    var franchiseAdmin = dbContext.FranchiseAdmins.FirstOrDefault(u => u.UserId == userId);
                     var workcategory = dbContext.WorkCategory.FirstOrDefault(w => w.Title == workCategoryDto.Title);
                     if (workcategory==null)
                    {
                        WorkCategory newWorkCategory = new WorkCategory
                        {
                            FranchiseId = franchiseAdmin.FranchiseId,
                            Title = workCategoryDto.Title
                        };

                        dbContext.WorkCategory.Add(newWorkCategory);
                        dbContext.SaveChanges();
                        return new WorkCategoryDto() {
                            Title= newWorkCategory.Title,
                            WorkCategoryId=newWorkCategory.WorkCategoryId
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

        public List<WorkCategoryDto> getWorkCategory(int userId)
        {
            using (var dbcontext = new OmContext())
            {
                try
                {
                    var WorkCategory = (from w in dbcontext.WorkCategory
                                       join fa in dbcontext.FranchiseAdmins on w.FranchiseId equals fa.FranchiseId
                                       where fa.UserId ==userId
                                       select new WorkCategoryDto()
                                       {
                                           WorkCategoryId = w.WorkCategoryId,
                                           FranchiseId = w.FranchiseId,
                                           Title = w.Title
                                       }).ToList();
                    return WorkCategory;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }
        public List<WorkCategoryDto> getCustomerWorkCategory(int userId)
        {
            using (var dbcontext = new OmContext())
            {
                try
                {
                    var WorkCategory = (from w in dbcontext.WorkCategory
                                        join fa in dbcontext.Customers on w.FranchiseId equals fa.FranchiseId
                                        where fa.UserId == userId
                                        select new WorkCategoryDto()
                                        {
                                            WorkCategoryId = w.WorkCategoryId,
                                            FranchiseId = w.FranchiseId,
                                            Title = w.Title
                                        }).ToList();
                    return WorkCategory;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }

        public WorkCategoryDto UpdateWorkCategory(WorkCategoryDto workCategoryDto)
        {
            using (var dbContext = new OmContext())
            {
                try
                {
                    var workCategory = dbContext.WorkCategory.First(c => c.WorkCategoryId == workCategoryDto.WorkCategoryId);
                    workCategory.Title = workCategoryDto.Title;

                    dbContext.SaveChanges();

                    return new WorkCategoryDto()
                    {
                        Title = workCategory.Title,
                        FranchiseId = workCategory.FranchiseId,
                        WorkCategoryId=workCategory.WorkCategoryId
                    };
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }
        public bool DeleteWorkCatgory(int categoryId)
        {
            using (var dbContext = new OmContext())
            {
                try
                {
                    var category = dbContext.WorkCategory.FirstOrDefault(u => u.WorkCategoryId == categoryId);
                    if (category != null)
                    {
                        dbContext.WorkCategory.Remove(category);
                        dbContext.SaveChanges();
                        return true;
                    }
                }
                catch(Exception e)
                {
                    return false;
                }
               
                return false;
            }
        }
    }
}