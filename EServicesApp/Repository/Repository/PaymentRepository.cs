using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Config;
using Repository.DTOModel;
using Data.Models;

namespace Repository.Repository
{
    public class PaymentRepository
    {
        public bool AddPayment(PaymentDto payment)
        {
            using (var dbContext = new OmContext())
            {
                try
                {
                    Payment newPayment = new Payment
                    {
                        JobId = payment.JobId,
                        PaidAmount = payment.PaidAmount
                    };
                    dbContext.Payments.Add(newPayment);
                    dbContext.SaveChanges();
                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
        }

        public UserPaymentInformationDto CustomerPayedAmount(int cusomterId)
        {
            using (var dbContext = new OmContext())
            {
                try
                {
                    var completedsum = dbContext.Jobs.Count(x => x.CustomerId == cusomterId && x.status == 2);
                    var reuestedsum = dbContext.Jobs.Count(x => x.CustomerId == cusomterId && x.status == 0);

                    var jobDtos = (from jb in dbContext.Jobs
                                   join py in dbContext.Payments on jb.JobId equals py.JobId
                                   where jb.CustomerId == cusomterId
                                   select new
                                   {
                                       jb.JobId,
                                       jb.CustomerId,
                                       py.PaidAmount
                                   }).ToList();

                    double salary = 0;

                    foreach (var item in jobDtos)
                    {
                        salary += item.PaidAmount;
                    };


                    UserPaymentInformationDto paymentInfo = new UserPaymentInformationDto
                    {
                        CompletedJobs = completedsum,
                        RequestedJobs = reuestedsum,
                        Amount = salary
                    };
                    return paymentInfo;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }
             public UserPaymentInformationDto WorkerPayedAmount(int workerId)
             {
            using (var dbContext = new OmContext())
            {
                try
                {
                    var completedsum = dbContext.Jobs.Count(x => x.WorkerId == workerId && x.status == 2);
                    var reuestedsum = dbContext.Jobs.Count(x => x.WorkerId == workerId && (x.status == 1 || x.status==0 || x.status==3));

                    var jobDtos = (from jb in dbContext.Jobs
                                   join py in dbContext.Payments on jb.JobId equals py.JobId
                                   where jb.WorkerId == workerId && jb.status==2
                                   select new
                                   {
                                       jb.JobId,
                                       jb.WorkerId,
                                       py.PaidAmount
                                   }).ToList();

                    double salary = 0;

                    foreach (var item in jobDtos)
                    {
                        salary += item.PaidAmount;
                    };


                    UserPaymentInformationDto paymentInfo = new UserPaymentInformationDto
                    {
                        CompletedJobs = completedsum,
                        RequestedJobs = reuestedsum,
                        Amount = salary
                    };
                    return paymentInfo;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }
    }
}
