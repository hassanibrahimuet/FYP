namespace Repository.DTOModel
{
    public class AddressDto
    {
        public int AddressId { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string Street { get; set; }
        public string House { get; set; }
        public string Longitude { get; set; }
        public string Latitude { get; set; }
    }
    public class LocationDto
    {
        public int LocationId { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
    }
}