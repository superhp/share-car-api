using System;
using System.Collections.Generic;
using System.Text;

namespace ShareCar.Dto.Identity.Cognizant
{
    public class VerificationCodeSubmitData
    {
        public int VerificationCode { get; set; }
        public string FacebookEmail { get; set; }
        public string GoogleEmail { get; set; }
    }
}
