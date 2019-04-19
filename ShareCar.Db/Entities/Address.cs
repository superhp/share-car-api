namespace ShareCar.Db.Entities
{
    public class Address
    {
        public int AddressId { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string Number { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
    }
}
