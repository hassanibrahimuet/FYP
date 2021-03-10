using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models
{
    public class NotificationDetail
    {
        public int NotificationDetailId { get; set; }
        public string Token { get; set; }
        public int UserId { get; set; }
        public virtual User Users { get; set; }
    }
}
