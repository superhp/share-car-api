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
        IEnumerable<PassengerDto> GetPassengersByRideId(int rideId);
        List<RideDto> GetFinishedPassengerRides(string passengerEmail);
        void SetRideAsInactive(RideDto ride);
        void AddRide(RideDto ride, string email);
        void UpdateRide(RideDto ride);
        IEnumerable<RouteDto> GetRoutes(RouteDto routeDto, string email);
        IEnumerable<RideDto> GetRidesByRoute(int routeId, string passengerEmail);
        bool IsRideRequested(int rideId, string email);
        bool IsDriver(int rideId, string email);
        bool IsPassenger(int rideId, string email);
    }
}
