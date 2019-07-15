using ShareCar.Db.Entities;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ShareCar.Db.Repositories.Ride_Repository
{
   public interface IRideRepository
    {
        Ride GetRideById(int id);
        IEnumerable<Ride> GetRidesByDriver(string email);
        IEnumerable<Passenger> GetPassengersByRideId(int rideId);
        void UpdateRide(Ride ride);
        void SetRideAsInactive(Ride ride);
        Ride AddRide(Ride ride);
       // IEnumerable<Ride> GetRidesByPassenger(Passenger passenger);
        //IEnumerable<Ride> GetRidesByRoute(int routeId);
        bool IsRideRequested(int rideId, string passengerEmail);
    }
}