using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ShareCar.Db.Entities
{
    public class RideRequestNote
    {
        public int RideRequestNoteId { get; set; }
        public string Text { get; set; }
        public int RideRequestId { get; set; }
        public RideRequest RideRequest { get; set; }
        public bool Seen { get; set; }
    }
}
