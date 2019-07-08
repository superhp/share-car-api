using ShareCar.Db.Entities;
using ShareCar.Dto.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShareCar.Db.Repositories.RideRequest_Repository
{
    public interface IRideRequestRepository
    {
        IEnumerable<RideRequest> GetAcceptedRequests(string passengerEmail);
        RideRequest AddRequest(RideRequest request);
        RideRequest GetRequestById(int id);
        IEnumerable<RideRequest> GetPassengerRequests(string email);
        IEnumerable<RideRequest> GetDriverRequests(string email);
        void UpdateRequest(RideRequest request);
        void SeenByPassenger(int[] requests);
        void SeenByDriver(int rideId);
        void DeletedRide(IEnumerable<RideRequest> requests);
        IEnumerable<RideRequest> GetRequestsByRideId(int rideId);

    }
}
