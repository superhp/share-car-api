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
using ShareCar.Logic.RideRequest_Logic;
using Microsoft.AspNetCore.Identity;
using ShareCar.Logic.Passenger_Logic;
using ShareCar.Db.Repositories.Ride_Repository;
using ShareCar.Logic.User_Logic;
using ShareCar.Logic.Note_Logic;
using ShareCar.Dto.Identity;
using Microsoft.EntityFrameworkCore.Internal;

namespace ShareCar.Logic.Ride_Logic
{
    public class RideLogic : IRideLogic
    {
        private readonly IRideRepository _rideRepository;
        private readonly IAddressLogic _addressLogic;
        private readonly IRouteLogic _routeLogic;
        private readonly IMapper _mapper;
        private readonly IUserLogic _userLogic;
        private readonly IDriverNoteLogic _driverNoteLogic;
        private readonly IPassengerLogic _passengerLogic;

        public RideLogic(IRouteLogic routeLogic, IRideRepository rideRepository, IDriverNoteLogic driverNoteLogic, IAddressLogic addressLogic, IMapper mapper, IUserLogic userLogic, IPassengerLogic passengerLogic)
        {
            _rideRepository = rideRepository;
            _addressLogic = addressLogic;
            _routeLogic = routeLogic;
            _mapper = mapper;
            _driverNoteLogic = driverNoteLogic;
            _userLogic = userLogic;
            _passengerLogic = passengerLogic;
        }

        public RideDto GetRideById(int id)
        {
            Ride ride = _rideRepository.GetRideById(id);

            if (ride == null)
            {
                return null;
            }

            return _mapper.Map<Ride, RideDto>(ride);
        }

        public IEnumerable<RideDto> GetRidesByDate(DateTime date)
        {
            IEnumerable<Ride> rides = _rideRepository.GetRidesByDate(date);


            return MapToList(rides);
        }

        public IEnumerable<RideDto> GetRidesByDriver(string email)
        {
            IEnumerable<Ride> rides = _rideRepository.GetRidesByDriver(email);

            List<RideDto> dtoRides = new List<RideDto>();

            var passengers = _passengerLogic.GetPassengersByDriver(email);

            var notes = _driverNoteLogic.GetNotesByDriver(email);

            List<UserDto> users = new List<UserDto>();

            foreach (var passenger in passengers.GroupBy(x => x.Email).Select(g => g.First()).ToList())
            {
                users.Add(_userLogic.GetUserByEmail(EmailType.LOGIN, passenger.Email));
            }

            foreach (var passenger in passengers)
            {
                var user = users.Single(x => x.Email == passenger.Email);
                passenger.FirstName = user.FirstName;
                passenger.LastName = user.LastName;
                passenger.Phone = user.Phone;
            }

            foreach (var ride in rides)
            {
                var dtoRide = _mapper.Map<Ride, RideDto>(ride);
                dtoRide.Route = _routeLogic.GetRouteById(ride.RouteId);
                dtoRide.Route.Rides = null;
                var note = notes.FirstOrDefault(x => x.RideId == ride.RideId);
                if(note != null)
                {
                    dtoRide.Note = note.Text;
                }
                dtoRide.Passengers = passengers.Where(x => x.RideId == ride.RideId).ToList();
                dtoRides.Add(dtoRide);
            }
            dtoRides = dtoRides.OrderByDescending(x => x.RideDateTime).ToList();
            return dtoRides;
        }

        public IEnumerable<RideDto> GetRidesByStartPoint(int addressFromId)
        {
            IEnumerable<Ride> rides = _rideRepository.GetRidesByStartPoint(addressFromId);
            return MapToList(rides);
        }

        public IEnumerable<RideDto> GetRidesByDestination(int addressToId)
        {
            IEnumerable<Ride> rides = _rideRepository.GetRidesByDestination(addressToId);
            return MapToList(rides);
        }

        public IEnumerable<PassengerDto> GetPassengersByRideId(int id)
        {
            IEnumerable<Passenger> passengers = _rideRepository.GetPassengersByRideId(id);

            return MapToList(passengers);
        }

        public void AddRide(RideDto ride, string email)
        {
            ride.DriverEmail = email;
            ride.Requests = new List<RideRequestDto>();
            AddRouteIdToRide(ride);
            var entity = _rideRepository.AddRide(_mapper.Map<RideDto, Ride>(ride));
            
            var note = _driverNoteLogic.AddNote(new DriverNoteDto { Text = ride.Note, RideId = entity.RideId });
            ride.DriverNoteId = note.DriverNoteId;
        }

        public void SetRideAsInactive(RideDto rideDto)
        {
            _rideRepository.SetRideAsInactive(_mapper.Map<RideDto, Ride>(rideDto));
        }

        public bool DoesUserBelongsToRide(string email, int rideId)
        {
            Ride ride = _rideRepository.GetRideById(rideId);

            if (ride == null)
            {
                return false;
            }

            if (ride.DriverEmail == email || ride.Passengers.Any(x => x.Email == email))
            {
                return true;
            }
            return false;
        }

        // Returns a list of mapped objects
        private IEnumerable<RideDto> MapToList(IEnumerable<Ride> rides)
        {
            List<RideDto> DtoRides = new List<RideDto>();

            foreach (var ride in rides)
            {
                DtoRides.Add(_mapper.Map<Ride, RideDto>(ride));
            }
            return DtoRides;
        }

        private IEnumerable<PassengerDto> MapToList(IEnumerable<Passenger> passengers)
        {
            List<PassengerDto> DtoPassengers = new List<PassengerDto>();

            foreach (var passenger in passengers)
            {
                DtoPassengers.Add(_mapper.Map<Passenger, PassengerDto>(passenger));
            }
            return DtoPassengers;
        }

        public IEnumerable<RideDto> GetSimilarRides(RideDto ride)
        {
            string driverEmail = ride.DriverEmail;
            int routeId = ride.RouteId;
            IEnumerable<Ride> rides = _rideRepository.GetSimmilarRides(driverEmail, routeId, ride.RideId);
            return MapToList(rides);
        }

        private void AddRouteIdToRide(RideDto ride)
        {
            if (ride.Route.FromAddress.Longitude != 0 && ride.Route.FromAddress.Latitude != 0 && ride.Route.ToAddress.Longitude != 0 && ride.Route.ToAddress.Latitude != 0)
            {
                RouteDto route = new RouteDto();
                route.FromId = _addressLogic.GetAddressId(ride.Route.FromAddress);
                route.ToId = _addressLogic.GetAddressId(ride.Route.ToAddress);
                route.Geometry = ride.Route.Geometry;
                route.FromAddress = ride.Route.FromAddress;
                route.ToAddress = ride.Route.ToAddress;

                int routeId = _routeLogic.GetRouteId(route.Geometry);
                if (routeId == -1)
                {
                    route.Geometry = ride.Route.Geometry;
                    _routeLogic.AddRoute(route);
                    ride.RouteId = _routeLogic.GetRouteId(route.Geometry);
                }
                else
                {
                    ride.RouteId = routeId;
                }
            }
        }

        public IEnumerable<RouteDto> GetRoutes(RouteDto routeDto, string email)
        {
            List<RouteDto> routes = _routeLogic.GetRoutes(routeDto, email);
            foreach (RouteDto route in routes)
            {
                AddDriversNamesToRouteRides(route.Rides);
            }
            return routes;

        }

        public List<RideDto> GetFinishedPassengerRides(string passengerEmail)
        {
            List<PassengerDto> passengers = _passengerLogic.GetUnrepondedPassengersByEmail(passengerEmail);

            //    IEnumerable<Ride> entityRides = _rideRepository.FindRidesByPassenger(_mapper.Map<PassengerDto,Passenger>(passengers));
            List<RideDto> dtoRides = new List<RideDto>();
            DateTime hourAfterRide = new DateTime();
            hourAfterRide = DateTime.Now;
            hourAfterRide.AddHours(1);
            foreach (PassengerDto passenger in passengers)
            {
                Ride ride = _rideRepository.GetRideById(passenger.RideId);
                if (ride != null)
                {
                    if (DateTime.Compare(ride.RideDateTime, hourAfterRide) < 0)
                    {

                        var user = _userLogic.GetUserByEmail(EmailType.LOGIN, ride.DriverEmail);
                        RideDto dtoRide = _mapper.Map<Ride, RideDto>(ride);
                        dtoRide.DriverFirstName = user.FirstName;
                        dtoRide.DriverLastName = user.LastName;
                        dtoRides.Add(dtoRide);
                    }
                }
            }
            return dtoRides;
        }

        public bool AddDriversNamesToRouteRides(List<RideDto> dtoRides)
        {
            List<string> emails = new List<string>();
            List<string> FirstNames = new List<string>();
            List<string> LastNames = new List<string>();
            List<string> phones = new List<string>();
            foreach (RideDto ride in dtoRides)
            {
                if (!emails.Contains(ride.DriverEmail))
                {
                    emails.Add(ride.DriverEmail);
                    var driver = _userLogic.GetUserByEmail(EmailType.LOGIN, ride.DriverEmail);
                    FirstNames.Add(driver.FirstName);
                    LastNames.Add(driver.LastName);
                    phones.Add(driver.Phone);
                    ride.DriverFirstName = driver.FirstName;
                    ride.DriverLastName = driver.LastName;
                    ride.DriverPhone = driver.Phone;
                }
                else
                {
                    int index = emails.IndexOf(ride.DriverEmail);
                    ride.DriverFirstName = FirstNames[index];
                    ride.DriverLastName = LastNames[index];
                    ride.DriverPhone = phones[index];
                }
            }
            return true;
        }

        public IEnumerable<RideDto> GetRidesByRoute(string routeGeometry)
        {
            IEnumerable<Ride> entityRides = _rideRepository.GetRidesByRoute(routeGeometry);

            List<RideDto> dtoRides = (List<RideDto>)MapToList(entityRides);

            AddDriversNamesToRouteRides(dtoRides);

            return dtoRides;
        }

        public void UpdateRide(RideDto ride)
        {
            _rideRepository.UpdateRide(_mapper.Map<RideDto, Ride>(ride));
        }

        public bool IsRideRequested(int rideId, string email)
        {
            return _rideRepository.IsRideRequested(rideId, email);
        }
    }
}