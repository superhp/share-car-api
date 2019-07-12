using System.Collections.Generic;
using ShareCar.Db.Entities;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System;

namespace ShareCar.Db.Repositories.RideRequest_Repository
{
    public class RideRequestRepository : IRideRequestRepository
    {

        private readonly ApplicationDbContext _databaseContext;

        public RideRequestRepository(ApplicationDbContext context)
        {
            _databaseContext = context;
        }

        public RideRequest AddRequest(RideRequest request)
        {
          var entity = _databaseContext.Requests.Add(request).Entity;
            _databaseContext.SaveChanges();
            return entity;
        }

        public IEnumerable<RideRequest> GetRequestsByRideId(int rideId)
        {
            return _databaseContext.Requests.Where(x => x.RideId == rideId && x.Status != Status.DELETED).ToList();
        }

        public void DeletedRide(IEnumerable<RideRequest> requests)
        {
            foreach (RideRequest request in requests)
            {
                RideRequest toUpdate = _databaseContext.Requests.Find(request.RideRequestId);
                if (toUpdate == null)
                {
                    throw new ArgumentException("User not found.");
                }
                toUpdate.SeenByPassenger = false;
                toUpdate.Status = Status.DELETED;
            }
            _databaseContext.SaveChanges();
        }

        public IEnumerable<RideRequest> GetPassengerRequests(string email)
        {
            return _databaseContext.Requests.Include(x => x.RequestedRide).Where(x => x.PassengerEmail == email && (x.SeenByPassenger == false || x.Status == Status.ACCEPTED || x.Status == Status.WAITING)).ToList();
        }

        public IEnumerable<RideRequest> GetDriverRequests(string email)
        {
            return _databaseContext.Requests.Where(x => x.DriverEmail == email && (x.Status == Status.WAITING || x.Status == Status.ACCEPTED || (x.Status == Status.CANCELED && !x.SeenByDriver))).ToList();
        }

        public RideRequest GetRequestById(int id)
        {
            return _databaseContext.Requests.Find(id);
        }

        public IEnumerable<RideRequest> GetAcceptedRequests(string passengerEmail)
        {
            return _databaseContext.Requests.Where(x => x.PassengerEmail == passengerEmail && x.Status == Status.DENIED).ToList();
        }

        public void SeenByDriver(int rideId)
        {
            var ride = _databaseContext.Rides.Include(x => x.Requests).Single(x => x.RideId == rideId);

            foreach(var request in ride.Requests)
            {
                request.SeenByDriver = true;
            }

            _databaseContext.SaveChanges();
        }

        public void SeenByPassenger(int[] requests)
        {
            IEnumerable<RideRequest> toUpdate = _databaseContext.Requests.Where(x => requests.Contains(x.RideRequestId));

            foreach (var request in toUpdate)
            {
                request.SeenByPassenger = true;
            }
            _databaseContext.SaveChanges();
        }

        public void UpdateRequest(RideRequest request)
        {
            RideRequest toUpdate = _databaseContext.Requests.Single(x => x.RideRequestId == request.RideRequestId);
            toUpdate.Status = request.Status;
            toUpdate.SeenByPassenger = request.SeenByPassenger;
            toUpdate.SeenByDriver = request.SeenByDriver;
            _databaseContext.Requests.Update(toUpdate);
            _databaseContext.SaveChanges();
        }

        public bool IsDriver(int requestId, string email)
        {
            var request = _databaseContext.Requests.Single(x => x.RideRequestId == requestId);
            var ride = _databaseContext.Rides.Single(x => x.RideId == request.RideId);
            return ride.DriverEmail == email;
        }
    }
}
