using System;
using System.Collections.Generic;
using System.Text;

namespace ShareCar.Logic.Exceptions
{
    public class RideInPastException : Exception
    {
        public RideInPastException()
    : base("Trying to create ride in the past")
        {

        }
    }
}

