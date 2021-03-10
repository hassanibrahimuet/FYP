namespace Repository.DTOModel
{
    public class FranchiseDto
    {
        public int FranchiseId { get; set; }
        public string FranchiseName { get; set; }
    }
    public class FranchiseUserDto:FranchiseDto
    {
        public SimpleUserDto FranchiseUser { get; set; }
    }
    public class FranchiseAdminDto
    {
        public int FranchiseId { get; set; }
        public int FranchiseAdminId { get; set; }
        public SimpleUserDto FranchiseAdmin { get; set; }
    }
    public class FranchiseWorkerDto : FranchiseDto
    {
        public SimpleUserDto FranchiseWorker { get; set; }
        public int WorkCategoryId { get; set; }
        public int WorkerId { get; set; }
    }
    public class FranchiseCustomerDto : FranchiseDto
    {
        public AddressDto CustomerAddres { get; set; }
        public SimpleUserDto FranchiseCustomer { get; set; }
        public int CustomerId { get; set; }
    }
}