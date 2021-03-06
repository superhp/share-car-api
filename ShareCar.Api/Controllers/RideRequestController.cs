﻿using System;
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
using ShareCar.Logic.User_Logic;

namespace ShareCar.Api.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [Route("api/RideRequest")]
    public class RideRequestController : Controller
    {
        private readonly IRideRequestLogic _requestLogic;
        private readonly IRideRequestNoteLogic _requestNoteLogic;
        private readonly IUserLogic _userLogic;
        private readonly IRideLogic _rideLogic;
        private readonly IDriverSeenNoteRepository _driverSeenNoteRepository;

        public RideRequestController(IRideRequestLogic requestLogic, IDriverSeenNoteRepository driverSeenNoteRepository, IUserLogic userLogic, IRideLogic rideLogic, IRideRequestNoteLogic noteLogic)
        {
            _requestLogic = requestLogic;
            _userLogic = userLogic;
            _rideLogic = rideLogic;
            _requestNoteLogic = noteLogic;
            _driverSeenNoteRepository = driverSeenNoteRepository;
        }

        [HttpGet("passenger")]
        public async Task<IActionResult> GetPassengerRequests()
        {
            var userDto = await _userLogic.GetLoggedInUser();

            IEnumerable<RideRequestDto> request = _requestLogic.GetPassengerRequests(userDto.Email);

            return Ok(request);
        }

        [HttpGet("noteSeenByPassenger/{requestId}")]
        public async Task<IActionResult> DriverSeenNoteAsync(int requestId)
        {
            await ValidatePassengerAsync(requestId);
            _driverSeenNoteRepository.NoteSeen(requestId);
            return Ok();
        }

        [HttpGet("NoteSeenByDriver/{requestId}")]
        public async Task<IActionResult> RequestNoteSeenAsync(int requestId)
        {
            await ValidateDriverAsync(requestId);
            _requestNoteLogic.NoteSeen(requestId);
            return Ok();
        }

        [HttpPost("updateNote")]
        public async Task<IActionResult> UpdateNoteAsync([FromBody] RideRequestNoteDto note)
        {
            await ValidatePassengerAsync(note.RideRequestId);
            _requestNoteLogic.UpdateNote(note);
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> AddRequest([FromBody] RideRequestDto request)
        {
            if (request == null)
            {
                return BadRequest();
            }
            var userDto = await _userLogic.GetLoggedInUser();
            request.PassengerEmail = userDto.Email;

            _requestLogic.AddRequest(request);

            return Ok();
        }

        [HttpPost("requestsSeenByPassenger")]
        public async Task RequestsSeenByPassenger([FromBody] int[] requests)
        {
            foreach(var request in requests)
            {
                await ValidatePassengerAsync(request);
            }
            _requestLogic.SeenByPassenger(requests);
        }

        [HttpGet("requestsSeenByDriver/{rideId}")]
        public void RequestsSeenByDriver(int rideId)
        {
            _requestLogic.SeenByDriver(rideId);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateRequestsAsync([FromBody] List<RideRequestDto> requests)
        {
            if (requests.Count == 0)
            {
                return BadRequest();
            }
            var userDto = await _userLogic.GetLoggedInUser();

            foreach (var request in requests)
            {
                _requestLogic.UpdateRequest(request, userDto.Email);
            }
            return Ok();
        }

        private async Task ValidatePassengerAsync(int rideRequestId)
        {
            var userDto = await _userLogic.GetLoggedInUser();
            if (!_requestLogic.IsRequester(rideRequestId, userDto.Email))
            {
                throw new UnauthorizedAccessException();
            }
        }
        private async Task ValidateDriverAsync(int requestId)
        {
            var userDto = await _userLogic.GetLoggedInUser();
            if (!_requestLogic.IsDriver(requestId, userDto.Email))
            {
                throw new UnauthorizedAccessException();
            }
        }
    }
}