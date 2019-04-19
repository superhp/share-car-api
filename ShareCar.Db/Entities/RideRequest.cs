using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ShareCar.Db.Entities
{
    public class RideRequest
    {
        [Key]
        public int RideRequestId { get; set; }
        public string PassengerEmail { get; set; }
        public string DriverEmail { get; set; }
        public int AddressId { get; set; }
        public int RideId { get; set; }
        public Status Status { get; set; }
        public bool SeenByDriver { get; set; }
        public bool SeenByPassenger { get; set; }
        [ForeignKey("RideId")]
        public virtual Ride RequestedRide { get; set; }
        [ForeignKey("PassengerEmail")]
        public virtual User Passenger { get; set; }
        [ForeignKey("DriverEmail")]
        public virtual User Driver { get; set; }
        [ForeignKey("AddressId")]
        public virtual Address RequestAddress { get; set; }
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
