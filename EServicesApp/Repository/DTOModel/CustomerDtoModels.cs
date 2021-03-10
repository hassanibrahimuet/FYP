namespace Repository.DTOModel
{
    public class CustomerDto:UserDto
    {
        public AddressDto CustomerAddress { set; get; }
        public int CustomerId { get; set; }
        public int UserId { get; set; }
        public int AddressId { get; set; }
        public int FranchiseId { get; set; }
    }

    public class CustomerLocation
    {
        public int UserId { get; set; }
        public string Longitude { get; set; }
        public string Latitude { get; set; }
    }
}
