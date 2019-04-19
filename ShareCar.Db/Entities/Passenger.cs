using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShareCar.Db.Entities
{
    public class Passenger
    {
        public int PassengerId { get; set; }
        public int RideId { get; set; }
        public bool Completed { get; set; }
        public string Email { get; set; }
        public bool PassengerResponded { get; set; }
        [ForeignKey("Email")]
        public virtual User User { get; set; }
        [ForeignKey("RideId")]
        public virtual Ride Ride { get; set; }
    }
}
