namespace Data.Models
{
    public class Franchise
    {
        public int FranchiseId { get; set; }
        public int UserId { get; set; }
        public virtual User User { get; set; }
        public string Name { get; set; }
    }

    public class FranchiseAdmin
    {
        public int FranchiseAdminId { get; set; }
        public int FranchiseId { get; set; }
        public virtual Franchise Franchise { get; set; }
        public int UserId { get; set; }
        public virtual User User { get; set; }
    }
}