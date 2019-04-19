using ShareCar.Dto.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShareCar.Dto
{
    public class PassengerDto
    {
        public string Email { get; set; }
        //public UserDto User { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public RouteDto Route { get; set; }
        public int RideId { get; set; }
        public RideDto Ride { get; set; }
        public bool Completed { get; set; }
        public string Phone { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
