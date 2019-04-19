using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShareCar.Db.Repositories;
using ShareCar.Dto;
using ShareCar.Logic.RideRequest_Logic;
using Microsoft.AspNetCore.Authorization;
using ShareCar.Logic.Ride_Logic;
using ShareCar.Db.Repositories.User_Repository;
using ShareCar.Logic.Exceptions;
using ShareCar.Logic.Note_Logic;
using ShareCar.Db.Repositories.Notes_Repository;

namespace ShareCar.Api.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [Route("api/RideRequest")]
    public class RideRequestController : Controller
    {
        private readonly IRideRequestLogic _requestLogic;
        private readonly IRideRequestNoteLogic _noteLogic;
        private readonly IUserRepository _userRepository;
        private readonly IRideLogic _rideLogic;
        private readonly IDriverSeenNoteRepository _driverSeenNoteRepository;

        public RideRequestController(IRideRequestLogic requestLogic, IDriverSeenNoteRepository driverSeenNoteRepository, IUserRepository userRepository, IRideLogic rideLogic, IRideRequestNoteLogic noteLogic)
        {
            _requestLogic = requestLogic;
            _userRepository = userRepository;
            _rideLogic = rideLogic;
            _noteLogic = noteLogic;
            _driverSeenNoteRepository = driverSeenNoteRepository;
        }

        [HttpGet("passenger")]
        public async Task<IActionResult> GetPassengerRequests()
        {
            var userDto = await _userRepository.GetLoggedInUser(User);

            IEnumerable<RideRequestDto> request = _requestLogic.GetPassengerRequests(userDto.Email);

            return Ok(request);
        }

        [HttpGet("{requestId}")]
        public IActionResult NoteSeen(int requestId)
        {
            _noteLogic.NoteSeen(requestId);
            return Ok();
        }

        [HttpPost("updateNote")]
        public IActionResult UpdateNote([FromBody] RideRequestNoteDto note)
        {
            _noteLogic.UpdateNote(note);
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> AddRequest([FromBody] RideRequestDto request)
        {
            if (request == null)
            {
                return BadRequest();
            }
            var userDto = await _userRepository.GetLoggedInUser(User);
            request.PassengerEmail = userDto.Email;
            var ride = _rideLogic.GetRideById(request.RideId);

            if(ride == null)
            {
                throw new RideNoLongerExistsException();
            }

            _requestLogic.AddRequest(request, ride.DriverEmail);

            return Ok();
        }

        [HttpPost("seenPassenger")]
        public void SeenRequestsPassenger([FromBody] int[] requests)
        {
            _requestLogic.SeenByPassenger(requests);
        }

        [HttpPost("seenDriver")]
        public void SeenDriverPassenger([FromBody] int[] requests)
        {
            _requestLogic.SeenByDriver(requests);
        }

        [HttpPut]
        public IActionResult UpdateRequests([FromBody] RideRequestDto request)
        {
            if (request == null)
            {
                return BadRequest();
            }

            _requestLogic.UpdateRequest(request);

            return Ok();

        }
    }
}