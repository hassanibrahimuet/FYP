namespace Data.Models
{
    public class Address
    {
        public int AddressId { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string Street { get; set; }
        public string House { get; set; }
        public string Longitude { get; set; }
        public string Latitude { get; set; }
    }

    public class Location
    {
        public int LocationId { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
    }
}