using ShareCar.Db.Entities;
using System;
using System.Collections.Generic;
using ShareCar.Dto;
using ShareCar.Logic.Address_Logic;
using AutoMapper;
using System.Linq;
using ShareCar.Db.Repositories;
using System.Threading.Tasks;
using ShareCar.Logic.Route_Logic;
using ShareCar.Logic.Ride_Logic;
using ShareCar.Db.Repositories.Passenger_Repository;
using ShareCar.Dto.Identity;
using ShareCar.Logic.User_Logic;

namespace ShareCar.Logic.Passenger_Logic
{
    public class PassengerLogic : IPassengerLogic
    {

        private readonly IPassengerRepository _passengerRepository;
        private readonly IAddressLogic _addressLogic;
      //  private readonly IRouteLogic _routeLogic;
        private readonly IMapper _mapper;

        public PassengerLogic(IMapper mapper, IPassengerRepository passengerRepository, IAddressLogic addressLogic)//, IRouteLogic routeLogic)
        {
            _mapper = mapper;
            _passengerRepository = passengerRepository;
            _addressLogic = addressLogic;
         //   _routeLogic = routeLogic;
        }

        public void AddPassenger(PassengerDto passenger)
        {
            _passengerRepository.AddNewPassenger(_mapper.Map<PassengerDto, Passenger>(passenger));
        }

        public List<PassengerDto> GetUnrepondedPassengersByEmail(string email)
        {
            IEnumerable<Passenger> passengers = _passengerRepository.GetUnrepondedPassengersByEmail(email);
            List<PassengerDto> dtoPassengers = new List<PassengerDto>();
            foreach (Passenger passenger in passengers)
            {
                dtoPassengers.Add(_mapper.Map<Passenger, PassengerDto>(passenger));
            }
            return dtoPassengers;
        }

        public List<PassengerDto> GetPassengersByDriver(string email)
        {
            IEnumerable<Passenger> passengers = _passengerRepository.GetPassengersByDriver(email);
            List<PassengerDto> dtoPassengers = new List<PassengerDto>();

            foreach (Passenger passenger in passengers)
            {
                passenger.Ride.Requests = passenger.Ride.Requests.Where(x => x.PassengerEmail == passenger.Email && x.Status == Db.Entities.Status.ACCEPTED).ToList();
                var dtoPassenger = _mapper.Map<Passenger, PassengerDto>(passenger);
                var address = _addressLogic.GetAddressById(passenger.Ride.Requests[0].AddressId);
             //   var route = _routeLogic.GetRouteByRequest(passenger.Ride.Requests[0].RideRequestId);
                dtoPassenger.Longitude = address.Longitude;
                dtoPassenger.Latitude = address.Latitude;
          //    dtoPassenger.Route = route;
                dtoPassenger.Ride = null;
                dtoPassenger.Route.Rides = null;
                dtoPassengers.Add(dtoPassenger);
            }
            return dtoPassengers;
        }
        public void RespondToRide(bool response, int rideId, string passengerEmail)
        {
            _passengerRepository.RespondToRide(response, rideId, passengerEmail);

        }
        public int GetUsersPoints(string email)
        {
            int points = _passengerRepository.GetUsersPoints(email);
            return points;
        }

        public void RemovePassenger(string email, int rideId)
        {
            _passengerRepository.RemovePassenger(email, rideId);
        }

        public bool IsUserAlreadyAPassenger(int rideId, string email)
        {
            return _passengerRepository.IsUserAlreadyAPassenger(rideId, email);
        }

        public void RemovePassengerByRide(int rideId)
        {
            _passengerRepository.RemovePassengerByRide(rideId);
        }
    }
}
