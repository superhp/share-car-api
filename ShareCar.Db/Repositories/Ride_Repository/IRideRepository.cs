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
        IEnumerable<Ride> GetRidesByDate(DateTime date);
        IEnumerable<Ride> GetRidesByDestination(int addressToId);
        IEnumerable<Ride> GetRidesByStartPoint(int addressFromId);
        IEnumerable<Passenger> GetPassengersByRideId(int rideId);
        void UpdateRide(Ride ride);
        void SetRideAsInactive(Ride ride);
        Ride AddRide(Ride ride);
        IEnumerable<Ride> GetSimmilarRides(string driverEmail, int routeId, int rideId);
        IEnumerable<Ride> GetRidesByPassenger(Passenger passenger);
        IEnumerable<Ride> GetRidesByRoute(string routeGeometry);
        bool IsRideRequested(int rideId, string passengerEmail);
    }
}