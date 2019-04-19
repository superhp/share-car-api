using System;
using System.Collections.Generic;
using System.Text;

namespace ShareCar.Db.Entities
{
    public class DriverSeenNote
    {
        public int DriverSeenNoteId { get; set; }
        public int DriverNoteId { get; set; }
        public DriverNote Note { get; set; }
        public int RideRequestId { get; set; }
        public RideRequest Request { get; set; }
        public bool Seen { get; set; }
      }
}
