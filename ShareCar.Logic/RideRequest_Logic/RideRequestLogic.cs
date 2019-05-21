using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using ShareCar.Db.Entities;
using ShareCar.Dto;
using ShareCar.Logic.Address_Logic;
using ShareCar.Logic.User_Logic;
using ShareCar.Logic.Ride_Logic;
using ShareCar.Db.Repositories;
using AutoMapper;
using ShareCar.Logic.Route_Logic;
using ShareCar.Logic.Passenger_Logic;
using ShareCar.Db.Repositories.RideRequest_Repository;
using System;
using System.Linq;
using ShareCar.Logic.Exceptions;
using ShareCar.Logic.Note_Logic;
using ShareCar.Db.Repositories.Notes_Repository;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace ShareCar.Logic.RideRequest_Logic
{
    public class RideRequestLogic : IRideRequestLogic
    {

        private readonly IRideRequestRepository _rideRequestRepository;
        private readonly IRideLogic _rideLogic;
        private readonly IPassengerLogic _passengerLogic;
        private readonly IAddressLogic _addressLogic;
        private readonly IRouteLogic _routeLogic;
        private readonly IMapper _mapper;
        private readonly IUserLogic _userLogic;
        private readonly IRideRequestNoteLogic _rideRequestNoteLogic;
        private readonly IDriverNoteLogic _driverNoteLogic;
        private readonly IDriverSeenNoteRepository _driverSeenNoteReposiotory;
        private readonly ClaimsPrincipal _user;
        public RideRequestLogic(IHttpContextAccessor httpContext, IDriverNoteLogic driverNoteLogic, IDriverSeenNoteRepository driverSeenNoteLogic, IRideRequestRepository rideRequestRepository, IRideRequestNoteLogic rideRequestNoteLogic, IAddressLogic addressLogic, IUserLogic userLogic, IMapper mapper, IPassengerLogic passengerLogic, IRideLogic rideLogic, IRouteLogic routeLogic)
        {
            _rideRequestRepository = rideRequestRepository;
            _addressLogic = addressLogic;
            _rideLogic = rideLogic;
            _userLogic = userLogic;
            _mapper = mapper;
            _passengerLogic = passengerLogic;
            _routeLogic = routeLogic;
            _rideRequestNoteLogic = rideRequestNoteLogic;
            _driverNoteLogic = driverNoteLogic;
            _driverSeenNoteReposiotory = driverSeenNoteLogic;
            _user = httpContext.HttpContext.User;

        }

        public void AddRequest(RideRequestDto requestDto)
        {
            var ride = _rideLogic.GetRideById(requestDto.RideId);

            if (ride == null)
            {
                throw new RideNoLongerExistsException();
            }

            if (ride.NumberOfSeats <= 0)
            {
                throw new NoSeatsInRideException();
            }

            if (_rideLogic.IsRideRequested(requestDto.RideId, requestDto.PassengerEmail))
            {
                throw new AlreadyRequestedException();
            }

            requestDto.SeenByDriver = false;
            requestDto.SeenByPassenger = true;
            requestDto.DriverEmail = ride.DriverEmail;
            int addressId = _addressLogic.GetAddressId(requestDto.Address);

            if(addressId == -1)
            {
                throw new ArgumentException("Failed to get address id");
            }

            requestDto.AddressId = addressId;
           var entity = _rideRequestRepository.AddRequest(_mapper.Map<RideRequestDto, RideRequest>(requestDto));

            var driverNote = _driverNoteLogic.GetNoteByRide(requestDto.RideId);


            _driverSeenNoteReposiotory.AddNote(new DriverSeenNote { RideRequestId = entity.RideRequestId, DriverNoteId = driverNote.DriverNoteId });

            if (requestDto.RequestNote != null)
            {
                _rideRequestNoteLogic.AddNote(new RideRequestNoteDto { RideRequestId = entity.RideRequestId, Text = requestDto.RequestNote });
            }
        }

        public void UpdateRequest(RideRequestDto request,string userEmail)
        {
            var entityRequest = _rideRequestRepository.GetRequestById(request.RideRequestId);
            var previousStatus = _mapper.Map<Db.Entities.Status, Dto.Status>(entityRequest.Status);

            if (entityRequest.DriverEmail != userEmail && entityRequest.PassengerEmail != userEmail)
            {
                throw new UnauthorizedAccessException();
            }

            var driver = userEmail == entityRequest.DriverEmail;

            if (driver)
            {
                if(request.Status == Dto.Status.WAITING || 
                    request.Status == Dto.Status.CANCELED ||
                    previousStatus == Dto.Status.CANCELED ||
                    previousStatus == Dto.Status.DELETED ||
                    previousStatus == Dto.Status.DENIED ||
                    (previousStatus == Dto.Status.ACCEPTED && request.Status == Dto.Status.DENIED))
                {
                    throw new UnauthorizedAccessException();
                }
            }
            else
            {
                if (previousStatus != Dto.Status.WAITING ||
                    request.Status == Dto.Status.ACCEPTED ||
                    request.Status == Dto.Status.DENIED || 
                    request.Status == Dto.Status.DELETED)
                {
                    throw new UnauthorizedAccessException();
                }
            }

            if(request.Status == previousStatus)
            {
                return;
            }

            if (request.Status == Dto.Status.CANCELED)
            {
                request.SeenByDriver = false;
                request.SeenByPassenger = true;

            }
            else
            {
                request.SeenByDriver = true;
                request.SeenByPassenger = false;
            }
            var rideToUpdate = _rideLogic.GetRideById(request.RideId);

            if (request.Status == Dto.Status.ACCEPTED && previousStatus == Dto.Status.WAITING)
            {
                if (rideToUpdate.NumberOfSeats != 0)
                {
                    if (_passengerLogic.IsUserAlreadyAPassenger(request.RideId, entityRequest.PassengerEmail))
                    {
                        throw new AlreadyAPassengerException();
                    }
                    else
                    {
                        _passengerLogic.AddPassenger(new PassengerDto { Email = entityRequest.PassengerEmail, RideId = request.RideId, Completed = false });
                        rideToUpdate.NumberOfSeats--;
                        _rideLogic.UpdateRide(rideToUpdate);
                    }
                }
                else
                {
                    throw new NoSeatsInRideException();
                }
            }
            else if (request.Status == Dto.Status.CANCELED && previousStatus == Dto.Status.ACCEPTED)
            {
                _passengerLogic.RemovePassenger(entityRequest.PassengerEmail, request.RideId);
                rideToUpdate.NumberOfSeats++;
                _rideLogic.UpdateRide(rideToUpdate);
            }
            _rideRequestRepository.UpdateRequest(_mapper.Map<RideRequestDto, RideRequest>(request));
        }

        void IRideRequestLogic.SeenByPassenger(int[] requests)
        {
            _rideRequestRepository.SeenByPassenger(requests);
        }

        void IRideRequestLogic.SeenByDriver(int[] requests)
        {
            _rideRequestRepository.SeenByDriver(requests);
        }

        public List<RideRequestDto> ConvertRequestsToDto(IEnumerable<RideRequest> entityRequests, bool isDriver)
        {
            List<RideRequestDto> dtoRequests = new List<RideRequestDto>();

            int count = 0;
            foreach (var request in entityRequests)
            {
                dtoRequests.Add(_mapper.Map<RideRequest, RideRequestDto>(request));
                var route = _routeLogic.GetRouteByRequest(request.RideRequestId);

                dtoRequests[count].Route = route;

                if (isDriver)
                {
                    var user = _userLogic.GetUserByEmail(EmailType.LOGIN, request.PassengerEmail);
                    dtoRequests[count].PassengerFirstName = user.FirstName;
                    dtoRequests[count].PassengerLastName = user.LastName;
                    dtoRequests[count].PassengerPhone = user.Phone;
                }
                else
                {
                    var user = _userLogic.GetUserByEmail(EmailType.LOGIN, request.DriverEmail);
                    dtoRequests[count].DriverFirstName = user.FirstName;
                    dtoRequests[count].DriverLastName = user.LastName;
                    dtoRequests[count].RideDateTime = request.RequestedRide.RideDateTime;
                }
                AddressDto address = _addressLogic.GetAddressById(request.AddressId);

                dtoRequests[count].Address = address;
                RideDto ride = _rideLogic.GetRideById(request.RideId);
                count++;
            }
            return dtoRequests;
        }

        //Changes request status to deleted
        public void DeletedRide(int rideId)
        {
            IEnumerable<RideRequest> entityRequests = _rideRequestRepository.GetRequestsByRideId(rideId);
            _rideRequestRepository.DeletedRide(entityRequests);
        }

        public List<RideRequestDto> GetAcceptedRequests(string passengerEmail)
        {
            IEnumerable<RideRequest> entityRequests = _rideRequestRepository.GetAcceptedRequests(passengerEmail);
            List<RideRequestDto> dtoRequests = new List<RideRequestDto>();
            foreach (RideRequest request in entityRequests)
            {
                dtoRequests.Add(_mapper.Map<RideRequest, RideRequestDto>(request));
            }
            return dtoRequests;
        }

        public IEnumerable<RideRequestDto> GetDriverRequests(string email)
        {
            IEnumerable<RideRequest> entityRequest;
            entityRequest = _rideRequestRepository.GetDriverRequests(email);

            IEnumerable<RideRequestDto> converted = ConvertRequestsToDto(entityRequest, true);

            foreach(var request in converted)
            {
                var note = _rideRequestNoteLogic.GetNoteByRideRequest(request.RideRequestId);
                if(note != null)
                {
                    request.RequestNote = note.Text;
                    request.RequestNoteSeen = note.Seen;
                }
                else
                {
                    request.RequestNoteSeen = true;
                }
            }

            return converted.OrderByDescending(x => !x.SeenByPassenger).ThenByDescending(x => x.Status == Dto.Status.WAITING).ThenByDescending(x => x.Status == Dto.Status.ACCEPTED).ToList();
        }

        public IEnumerable<RideRequestDto> GetPassengerRequests(string email)
        {
            IEnumerable<RideRequest>  entityRequest = _rideRequestRepository.GetPassengerRequests(email);

            var notes = _rideRequestNoteLogic.GetNoteByPassenger(email);
            var driverSeenNotes =_driverSeenNoteReposiotory.GetNotesByPassenger(email);
            IEnumerable<RideRequestDto> converted = ConvertRequestsToDto(entityRequest, false);

            foreach(var request in converted)
            {

                var note = notes.FirstOrDefault(x => x.RideRequestId == request.RideRequestId);
                var driverNote = _driverNoteLogic.GetNoteByRide(request.RideId);

                if(driverNote != null)
                {
                    request.RideNote = driverNote.Text;
                    request.RideNoteSeen = driverSeenNotes.Single(x => x.RideRequestId == request.RideRequestId).Seen;
                }
                else
                {
                    request.RideNoteSeen = true;
                }

                if (note != null)
                {
                    request.RequestNote = note.Text;
                }
            }

            return converted.OrderByDescending(x => !x.SeenByPassenger).ThenByDescending(x => x.Status == Dto.Status.WAITING).ThenByDescending(x => x.Status == Dto.Status.ACCEPTED).ToList();
        }

        public bool IsRequester(int rideRequestId, string email)
        {
            var request = _rideRequestRepository.GetRequestById(rideRequestId);
            return request.PassengerEmail == email;
        }
    }
}
