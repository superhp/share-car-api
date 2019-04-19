using System;
using System.Collections.Generic;
using System.Text;

namespace ShareCar.Logic.Exceptions
{
    public class RideNoLongerExistsException: Exception
    {
        public RideNoLongerExistsException()
    : base("Ride no longer exists")
        {

        }
    }
}
