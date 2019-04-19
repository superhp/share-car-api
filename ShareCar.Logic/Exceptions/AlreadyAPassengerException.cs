using System;
using System.Collections.Generic;
using System.Text;

namespace ShareCar.Logic.Exceptions
{
    public class AlreadyAPassengerException : Exception
    {
        public AlreadyAPassengerException()
    : base("This user is already a passenger of the ride")
        {

        }
    }
}

