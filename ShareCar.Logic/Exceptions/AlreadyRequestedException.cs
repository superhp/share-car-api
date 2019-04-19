using System;
using System.Collections.Generic;
using System.Text;

namespace ShareCar.Logic.Exceptions
{
    public class AlreadyRequestedException : Exception
    {
        public AlreadyRequestedException()
            : base("Ride is already requested")
        {

        }
    }
}
