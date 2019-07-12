using System;
using System.Collections.Generic;
using System.Text;

namespace ShareCar.Dto
{
    public class RouteRideDto
    {
        public int RideId { get; set; }
        public DateTime RideDateTime { get; set; }
        public bool Requested { get; set; }
    }
}
