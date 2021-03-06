﻿using System.Collections.Generic;
using System.Threading.Tasks;
using ShareCar.Dto;


namespace ShareCar.Logic.RideRequest_Logic
{
    public interface IRideRequestLogic
    {
        IEnumerable<RideRequestDto> GetPassengerRequests(string email);
        IEnumerable<RideRequestDto> GetDriverRequests(string email);
        void UpdateRequest(RideRequestDto request, string userEmail);
        void AddRequest(RideRequestDto request);
        void SeenByPassenger(int[] requests);
        void SeenByDriver(int rideId);
        void DeletedRide(int rideId);
        bool IsRequester(int rideRequestId, string email);
        bool IsDriver(int requestId, string email);
    }
}
