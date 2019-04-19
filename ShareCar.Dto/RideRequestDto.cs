using ShareCar.Dto.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShareCar.Dto
{
    public class RideRequestDto
    {
        public int RideRequestId { get; set; }
        public string PassengerEmail { get; set; }
        public string DriverEmail { get; set; }
        public string DriverFirstName { get; set; }
        public string DriverLastName { get; set; }
        public string PassengerFirstName { get; set; }
        public string PassengerLastName { get; set; }
        public string PassengerPhone { get; set; }
        public AddressDto Address { get; set; }
        public bool SeenByDriver { get; set; }
        public bool SeenByPassenger { get; set; }
        public int RideId { get; set; }
        public string RequestNote { get; set; }
        public bool RequestNoteSeen { get; set; }
        public string RideNote { get; set; }
        public bool RideNoteSeen { get; set; }
        public int RideRequestNoteId { get; set; }
        public int AddressId { get; set; }
        public Status Status { get; set; }
        public RouteDto Route { get; set; }
        public DateTime RideDateTime { get; set; }
    }
    public enum Status
    {
        WAITING,
        ACCEPTED,
        DENIED,
        DELETED,
        CANCELED
    }
}
