namespace Repository.DTOModel
{
    public class WorkCategoryDto
    {
        public int WorkCategoryId { get; set; }
        public string Title { get; set; }
        public int FranchiseId { get; set; }
    }

    public class ServiceDto
    {
        public int ServiceId { get; set; }
        public string Title { get; set; }
        public double Price { get; set; }
        public int WorkCategoryId { get; set; }
    }
}