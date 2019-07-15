using ShareCar.Db.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShareCar.Db.Repositories.Passenger_Repository
{
    public interface IPassengerRepository
    {
        int GetUsersPoints(string email);
        void AddNewPassenger(Passenger passenger);
        void RemovePassenger(string email, int rideId);
        IEnumerable<Passenger> GetUnrepondedPassengersByEmail(string email);
    //    IEnumerable<Passenger> GetPassengersByDriver(string email);
        void RespondToRide(bool response, int rideId, string passengerEmail);
        bool IsUserAlreadyAPassenger(int rideId, string email);
        void RemovePassengerByRide(int rideId);
    }
}
