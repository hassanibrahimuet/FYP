using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models
{
    public class Payment
    {
        public int PaymentId { get; set; }
        public int JobId { get; set; }
        public virtual Job Job { get; set; }
        public double PaidAmount { get; set;}
    }
}
