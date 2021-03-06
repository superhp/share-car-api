﻿using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShareCar.Db.Entities
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public long? FacebookId { get; set; }
        public long? GoogleId { get; set; }
        public string FacebookEmail { get; set; }
        public string GoogleEmail { get; set; }
        public bool FacebookVerified { get; set; }
        public bool GoogleVerified { get; set; }
        public string CognizantEmail { get; set; }
        public string PictureUrl { get; set; }
        public string LicensePlate { get; set; }
        public string CarColor { get; set; }
        public string CarModel { get; set; }
        public int NumberOfSeats { get; set; }
        public int? HomeAddressId { get; set; }
        [ForeignKey("HomeAddressId")]
        public virtual Address HomeAddress { get; set; }
        public string Phone { get; set; }
    }
}