using System;
using System.Collections.Generic;
using System.Text;

namespace ShareCar.Dto
{
    public class RideDto
    {
        public int RideId { get; set; }
        public int RouteId { get; set; }
        public bool Requested { get; set; }
        public RouteDto Route { get; set; }
        public string DriverEmail { get; set; }
        public string DriverFirstName { get; set; }
        public string DriverLastName { get; set; }
        public string DriverPhone { get; set; }
        public DateTime RideDateTime { get; set; }
        public string Note { get; set; }
        public int DriverNoteId { get; set; }
        public List<RideRequestDto> Requests { get; set; }
        public List<PassengerDto> Passengers { get; set; }
        public int NumberOfSeats { get; set; }
        public bool isActive { get; set; }


    }
}
