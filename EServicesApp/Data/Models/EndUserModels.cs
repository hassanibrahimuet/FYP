namespace Data.Models
{
    public class EndUser
    {
        public int EndUserId { get; set; }
        public int UserId { get; set; }
        public virtual User User { get; set; }
        public int AddressId { get; set; }
        public virtual Address Address { get; set; }
        public int FranchiseId { get; set; }
        public virtual Franchise Franchise { get; set; }
    }
}