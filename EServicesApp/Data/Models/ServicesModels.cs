namespace Data.Models
{
    public class WorkCategory
    {
        public int WorkCategoryId { get; set; }
        public string Title { get; set; }
        public int FranchiseId { get; set; }
        public virtual Franchise Franchise { get; set; }
    }

    public class Service
    {
        public int ServiceId { get; set; }
        public string Title { get; set; }
        public double Price { get; set; }
        public int WorkCategoryId { get; set; }
        public virtual WorkCategory WorkCategory { get; set; }
    }
}