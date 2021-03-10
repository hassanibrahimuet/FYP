using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.DTOModel
{
    public class PaymentDto
    {
        public int PaymentId { get; set; }
        public int JobId { get; set; }
        public double PaidAmount { get; set; }
    }

    public class UserPaymentInformationDto
    {
        public int CompletedJobs { get; set; }
        public int RequestedJobs { get; set; }
        public double Amount { get; set; }
    }
}
