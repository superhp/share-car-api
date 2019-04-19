using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShareCar.Db.Entities
{
    public class Ride
    {
        public int RideId { get; set; }
        public int RouteId { get; set; }
        public string DriverEmail { get; set; }
        [DefaultValue (4)]
        public int NumberOfSeats { get; set; }
        public bool isActive { get; set; }
        public List<Passenger> Passengers { get; set; }
        public List<RideRequest> Requests { get; set; }
        public DateTime RideDateTime { get; set; }
        [ForeignKey("RouteId")]
        public virtual Route Route { get; set; }
    }
}
