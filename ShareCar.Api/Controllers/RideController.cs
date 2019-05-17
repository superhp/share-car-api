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

        [HttpPost("updateNote")]
        public async Task<IActionResult> UpdateNoteAsync([FromBody]DriverNoteDto note)
        {
            await ValidateDriverAsync(note.RideId);
             _driverNoteLogic.UpdateNote(note);
             return Ok();
        }


        [HttpPost("passengerResponse")]
        public async Task<IActionResult> PassengerResponseAsync([FromBody]PassengerResponseDto response)
        {

            var userDto = await _userRepository.GetLoggedInUser(User);
            await ValidatePassengerAsync(response.RideId);
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

        [HttpGet("RidesByRoute/{routeId}")]
        public async Task<IActionResult> GetRidesByRouteAsync(int routeId)
        {
            var userDto = await _userRepository.GetLoggedInUser(User);
            IEnumerable<RideDto> rides = _rideLogic.GetRidesByRoute(routeId, userDto.Email);
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
            await ValidateDriverAsync(rideId);

            IEnumerable<PassengerDto> passengers = _rideLogic.GetPassengersByRideId(rideId);
            return Ok(passengers);

        }

        [HttpPut("disactivate")]
        public async Task<IActionResult> SetRideAsInactive([FromBody] RideDto rideDto)
        {
            await ValidateDriverAsync(rideDto.RideId);
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
        private async Task ValidateDriverAsync(int rideId)
        {
            var userDto = await _userRepository.GetLoggedInUser(User);
            if (!_rideLogic.IsDriver(rideId, userDto.Email))
            {
                throw new UnauthorizedAccessException();
            }
        }

        private async Task ValidatePassengerAsync(int rideId)
        {
            var userDto = await _userRepository.GetLoggedInUser(User);
            if (!_rideLogic.IsPassenger(rideId, userDto.Email))
            {
                throw new UnauthorizedAccessException();
            }
        }
    }


}