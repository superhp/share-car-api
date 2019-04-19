using System;
using System.Collections.Generic;
using System.Text;

namespace ShareCar.Dto
{
    public class EmailFilter
    {
        public string Email { get; set; }
        public EmailType Type { get; set; }
    }
    public enum EmailType
    {
        FACEBOOK,
        GOOGLE,
        LOGIN,
        COGNIZANT
    }
}
