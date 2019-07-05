using System;
using System.Collections.Generic;
using System.Text;

namespace ShareCar.Dto
{
    public class RouteDto
    {
        public int RouteId { get; set; }
        public int FromId { get; set; }
        public int ToId { get; set; }
        public string Geometry { get; set; }
        public List<RouteRideDto> Rides { get; set; }
        public DateTime FromTime { get; set; }
        public AddressDto FromAddress { get; set; }
        public AddressDto ToAddress { get; set; }
        public int AddressFromId { get; set; }
        public int AddressToId { get; set; }
        public string DriverFirstName { get; set; }
        public string DriverLastName { get; set; }
        public string DriverEmail { get; set; }
    }

}
