namespace Repository.DTOModel
{
    public class WorkerDto:UserDto
    {
        public int WorkerId { get; set; }
        public int FranchiseId { get; set; }
        public int UserId { get; set; }
        public int WorkCategoryId { get; set; }
        public string FranchiseName { get; set; }
        public string Longitude { get; set; }
        public string Latitude { get; set; }
        public int status { get; set; }
        public float Rate { get; set; }
    }

    public class NearsetWorker
    {
        public int WorkerId { get; set; }
        public double distance { get; set; }
        public float Rate { get; set; }
    }
}