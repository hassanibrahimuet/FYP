using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.DTOModel
{
    public class SuperAdminDetialDto
    {
        public int SuperAdmins { get; set; }
        public int Franchises { get; set; }
        public int Customers { get; set; }
        public int Workers { get; set; }
    }

    public class FranchiseDetailDto
    {
        public int Admins { get; set; }
        public int Customers { get; set; }
        public int Workers { get; set; }
        public int Categories { get; set; }
        public int Services { get; set; }
    }
}
