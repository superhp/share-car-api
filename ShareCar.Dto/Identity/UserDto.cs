namespace ShareCar.Dto.Identity
{
    public class UserDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public long? FacebookId { get; set; }
        public long? GoogleId { get; set; }
        public string FacebookEmail { get; set; }
        public string GoogleEmail { get; set; }
        public bool FacebookVerified { get; set; }
        public bool GoogleVerified { get; set; }
        public string CognizantEmail { get; set; }
        public string PictureUrl { get; set; }
        public string Email { get; set; }
        public string LicensePlate { get; set; }
        public string Phone { get; set; }
        public string CarColor { get; set; }
        public string CarModel { get; set; }
        public int NumberOfSeats { get; set; }
    }
}