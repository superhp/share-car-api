using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using ShareCar.Db.Entities;
using ShareCar.Db.Repositories;
using ShareCar.Db.Repositories.Notes_Repository;
using ShareCar.Db.Repositories.User_Repository;
using ShareCar.Dto;
using ShareCar.Logic.Address_Logic;
using ShareCar.Logic.Note_Logic;
using ShareCar.Logic.Passenger_Logic;
using ShareCar.Logic.Ride_Logic;
using ShareCar.Logic.RideRequest_Logic;
using ShareCar.Logic.Route_Logic;

namespace ShareCar.Api.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [Route("api/Ride")]
    public class RideController : Controller
    {
        private readonly IRideLogic _rideLogic;
        private readonly IRouteLogic _routeLogic;
        private readonly IRideRequestLogic _rideRequestLogic;
        private readonly IUserRepository _userRepository;
        private readonly IPassengerLogic _passengerLogic;
        private readonly IDriverNoteLogic _driverNoteLogic;
        private readonly IAddressLogic _addressLogic;
        private readonly IDriverSeenNoteRepository _driverSeenNoteRepository;

        public RideController(IAddressLogic addressLogic, IRideRequestLogic rideRequestLogic, IDriverSeenNoteRepository driverSeenNoteRepository, IRideLogic rideLogic, IRouteLogic routeLogic, IUserRepository userRepository, IPassengerLogic passengerLogic, IDriverNoteLogic driverNoteLogic)
        {
            _addressLogic = addressLogic;
            _rideLogic = rideLogic;
            _rideRequestLogic = rideRequestLogic;
            _routeLogic = routeLogic;
            _userRepository = userRepository;
            _passengerLogic = passengerLogic;
            _driverNoteLogic = driverNoteLogic;
            _driverSeenNoteRepository = driverSeenNoteRepository;
        }
        [HttpGet("simillarRides={rideId}")]
        public IActionResult GetSimillarRides(int rideId)
        {
            RideDto ride = _rideLogic.GetRideById(rideId);

            IEnumerable<RideDto> rides = _rideLogic.GetSimilarRides(ride);
            return Ok(rides);
        }

        [HttpPost("updateNote")]
        public IActionResult UpdateNote([FromBody]DriverNoteDto note)
        {
             _driverNoteLogic.UpdateNote(note);
             return Ok();
        }

        [HttpGet("{requestId}")]
        public IActionResult SeenNote(int requestId)
        {
            _driverSeenNoteRepository.NoteSeen(requestId);
            return Ok();
        }


        [HttpPost("passengerResponse")]
        public async Task<IActionResult> PassengerResponseAsync([FromBody]PassengerResponseDto response)
        {
            var userDto = await _userRepository.GetLoggedInUser(User);
            _passengerLogic.RespondToRide(response.Response, response.RideId, userDto.Email);

            return Ok();
        }

        [HttpGet("checkFinished")]
        public async Task<IActionResult> CheckForFinishedRidesAsync()
        {
            var userDto = await _userRepository.GetLoggedInUser(User);

            List<RideDto> rides = _rideLogic.GetFinishedPassengerRides(userDto.Email);
            return Ok(rides);
        }

        [HttpGet]
        public async Task<IActionResult> GetRidesByLoggedUser()
        {
            var userDto = await _userRepository.GetLoggedInUser(User);
            List<RideDto> rides = (List<RideDto>)_rideLogic.GetRidesByDriver(userDto.Email);

            var requests = _rideRequestLogic.GetDriverRequests(userDto.Email);

              foreach (var ride in rides)
            {
                ride.Requests = requests.Where(x => x.RideId == ride.RideId).ToList();
            }

            return Ok(rides);
        }

        [HttpGet("ridedate={rideDate}")]
        public IActionResult GetRidesByDate(DateTime rideDate)
        {
            IEnumerable<RideDto> rides = _rideLogic.GetRidesByDate(rideDate);
            return Ok(rides);
        }

        [HttpGet("addressFromId={addressFromId}")]
        public IActionResult GetRidesByStartPoint(int addressFromId)
        {
            IEnumerable<RideDto> rides = _rideLogic.GetRidesByStartPoint(addressFromId);
            return Ok(rides);
        }

        [HttpGet("addressToId={addressToId}")]
        public IActionResult GetRidesByDestination(int addressToId)
        {
            IEnumerable<RideDto> rides = _rideLogic.GetRidesByDestination(addressToId);
            return Ok(rides);
        }

        [HttpGet("ridesByRoute={routeGeometry}")]
        public IActionResult GetRidesRouteAsync(string routeGeometry)
        {
            IEnumerable<RideDto> rides = _rideLogic.GetRidesByRoute(routeGeometry);
            return Ok(rides);
        }

        [HttpPost("routes")]
        public async Task<IActionResult> GetRoutesAsync([FromBody]RouteDto routeDto)
        {

            if (routeDto.FromAddress == null && routeDto.ToAddress == null)
            { 
            return BadRequest();
        }
            var userDto = await _userRepository.GetLoggedInUser(User);
            IEnumerable<RouteDto> routes = _rideLogic.GetRoutes(routeDto, userDto.Email);

            return Ok(routes);
        }

        [HttpGet("rideId={rideId}")]
        public async Task<IActionResult> GetPassengersByRideAsync(int rideId)
        {
            var userDto = await _userRepository.GetLoggedInUser(User);
            if (!_rideLogic.DoesUserBelongsToRide(userDto.Email, rideId))
            {
                return Unauthorized();
            }

            IEnumerable<PassengerDto> passengers = _rideLogic.GetPassengersByRideId(rideId);
            return Ok(passengers);

        }

        [HttpPut("disactivate")]
        public async Task<IActionResult> SetRideAsInactive([FromBody] RideDto rideDto)
        {

            var userDto = await _userRepository.GetLoggedInUser(User);
            if (rideDto == null)
            {
                return BadRequest();
            }
            _passengerLogic.RemovePassengerByRide(rideDto.RideId);
            _rideRequestLogic.DeletedRide(rideDto.RideId);
            _rideLogic.SetRideAsInactive(rideDto);
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> AddRides([FromBody] IEnumerable<RideDto> rides)
        {
            var userDto = await _userRepository.GetLoggedInUser(User);
            if (rides == null)
            {
                return BadRequest();
            }
            foreach (var ride in rides)
            {
                _rideLogic.AddRide(ride, userDto.Email);
            }
            return Ok();

        }
    }


}