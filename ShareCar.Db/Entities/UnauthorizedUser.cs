using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ShareCar.Db.Entities
{
    public class UnauthorizedUser
    {
        public int UnauthorizedUserId { get; set; }
        public int VerificationCode { get; set; }
        public string Email { get; set; }
        [ForeignKey("Email")]
        public virtual User User { get; set; }
    }
}
