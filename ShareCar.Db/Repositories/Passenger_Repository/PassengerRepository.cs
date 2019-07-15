using Microsoft.EntityFrameworkCore;
using ShareCar.Db.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace ShareCar.Db.Repositories.Passenger_Repository
{
    public class PassengerRepository : IPassengerRepository
    {
        private readonly ApplicationDbContext _databaseContext;

        public PassengerRepository(ApplicationDbContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public int GetUsersPoints(string email)
        {
            int points = _databaseContext.Passengers
                .Join(_databaseContext.Rides.Where(x => ((x.RideDateTime.Month == DateTime.Now.Month && x.RideDateTime.Year == DateTime.Now.Year) && (x.DriverEmail == email))),
                x => x.RideId,
                y => y.RideId,
                (x, y) => x).Where(z => z.Completed == true).Count();
            return points;
        }

        public void AddNewPassenger(Passenger passenger)
        {
                _databaseContext.Passengers.Add(passenger);
                _databaseContext.SaveChanges();

        }

        public IEnumerable<Passenger> GetUnrepondedPassengersByEmail(string email)
        {
                return _databaseContext.Passengers.Where(x => x.Email == email && x.PassengerResponded == false);

        }

        /*public IEnumerable<Passenger> GetPassengersByDriver(string email)
        {
            return _databaseContext.Passengers.Include(x => x.Ride.Requests).Where(x => x.Ride.DriverEmail == email);
        }*/

        public void RespondToRide(bool response, int rideId, string passengerEmail)
        {
                Passenger passenger = _databaseContext.Passengers.Single(x => x.RideId == rideId && x.Email == passengerEmail);
                passenger.PassengerResponded = true;
                passenger.Completed = response;
                _databaseContext.SaveChanges();

            }

        public void RemovePassenger(string email, int rideId)
        {
            Passenger passenger = _databaseContext.Passengers.Single(x => x.RideId == rideId && x.Email == email);
            _databaseContext.Remove(passenger);
        }

        public bool IsUserAlreadyAPassenger(int rideId, string email)
        {
            var passenger = _databaseContext.Passengers.FirstOrDefault(x => x.Email == email && x.RideId == rideId);

            return passenger != null;
        }

        public void RemovePassengerByRide(int rideId)
        {
            var passengers = _databaseContext.Passengers.Where(x => x.RideId == rideId);
            foreach (var passenger in passengers)
            {
                _databaseContext.Remove(passenger);
            }
        }
    }
}