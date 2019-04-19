using System;
using System.Collections.Generic;
using System.Text;

namespace ShareCar.Dto
{
    public class RideRequestNoteDto
    {
        public int RideRequestNoteId { get; set; }
        public string Text { get; set; }
        public string PassengerEmail { get; set; }
        public int RideId { get; set; }
        public int RideRequestId { get; set; }
        public bool Seen { get; set; }
    }
}
