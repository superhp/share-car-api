using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ShareCar.Dto;
using ShareCar.Dto.Identity;

namespace ShareCar.Logic.Ride_Logic
{
    public interface IRideLogic
    {
        IEnumerable<RideDto> GetRidesByDriver(string email);
        RideDto GetRideById(int id);
        IEnumerable<RideDto> GetRidesByDate(DateTime date);
        IEnumerable<RideDto> GetRidesByDestination(int addressToId);
        IEnumerable<RideDto> GetRidesByStartPoint(int adressFromId);
        IEnumerable<PassengerDto> GetPassengersByRideId(int rideId);
        bool DoesUserBelongsToRide(string email, int rideId);
        List<RideDto> GetFinishedPassengerRides(string passengerEmail);
        void SetRideAsInactive(RideDto ride);
        void AddRide(RideDto ride, string email);
        void UpdateRide(RideDto ride);
        IEnumerable<RideDto> GetSimilarRides(RideDto ride);
        IEnumerable<RouteDto> GetRoutes(RouteDto routeDto, string email);
        IEnumerable<RideDto> GetRidesByRoute(string routeGeometry);
        bool IsRideRequested(int rideId, string email);
    }
}
