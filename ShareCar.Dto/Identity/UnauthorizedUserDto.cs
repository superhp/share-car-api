using System;
using System.Collections.Generic;
using System.Text;

namespace ShareCar.Dto.Identity
{
    public class UnauthorizedUserDto
    {
        public int UnauthorizedUserId { get; set; }
        public int VerificationCode { get; set; }
        public string Email { get; set; }
    }
}
