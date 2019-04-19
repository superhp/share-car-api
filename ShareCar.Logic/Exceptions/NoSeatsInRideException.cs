using System;
using System.Collections.Generic;
using System.Text;

namespace ShareCar.Logic.Exceptions
{
    public class NoSeatsInRideException : Exception
    {
        public NoSeatsInRideException()
    : base("Ride doesn't have any seats left")
        {

        }
    }
}
