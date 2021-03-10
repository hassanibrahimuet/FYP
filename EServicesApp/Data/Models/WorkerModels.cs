namespace Data.Models
{
    public class Worker
    {
        public int WorkerId { get; set; }
        public int FranchiseId { get; set; }
        public virtual Franchise Franchise { get; set; }
        public int UserId { get; set; }
        public virtual User User { get; set; }
        public int WorkCategoryId { get; set; }
        public virtual WorkCategory WorkCategory { get; set; }
        public string Longitude { get; set; }
        public string Latitude { get; set; }
        public int status { get; set; }
        public float Rate { get; set; }
    }
}