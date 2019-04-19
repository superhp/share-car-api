using AutoMapper;
using ShareCar.Db.Entities;
using ShareCar.Db.Repositories.Notes_Repository;
using ShareCar.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShareCar.Logic.Note_Logic
{
    public class RideRequestNoteLogic : IRideRequestNoteLogic
    {
        private readonly IRideRequestNoteRepository _rideRequestNoteRepository;
        private readonly IMapper _mapper;

        public RideRequestNoteLogic(IRideRequestNoteRepository rideRequestNoteRepository, IMapper mapper)
        {
            _rideRequestNoteRepository = rideRequestNoteRepository;
            _mapper = mapper;
        }

        public void AddNote(RideRequestNoteDto note)
        {
            _rideRequestNoteRepository.AddNote(_mapper.Map<RideRequestNoteDto, RideRequestNote>(note));
        }

        public List<RideRequestNoteDto> GetNoteByPassenger(string email)
        {
            return _mapper.Map<List<RideRequestNote>, List<RideRequestNoteDto>>(_rideRequestNoteRepository.GetNoteByPassenger(email).ToList());
        }

        public RideRequestNoteDto GetNoteByRide(int rideId)
        {
            var entityNote = _rideRequestNoteRepository.GetNoteByRide(rideId);
            if (entityNote == null)
            {
                return null;
            }
            var dtoNote = _mapper.Map<RideRequestNote, RideRequestNoteDto>(entityNote);
            dtoNote.RideId = entityNote.RideRequest.RideId;
            return dtoNote;
        }

        public RideRequestNoteDto GetNoteByRideRequest(int rideRequestId)
        {
            var entityNote = _rideRequestNoteRepository.GetNoteByRideRequest(rideRequestId);
            if(entityNote == null)
            {
                return null;
            }
            var dtoNote = _mapper.Map<RideRequestNote, RideRequestNoteDto>(entityNote);
            dtoNote.RideRequestId = entityNote.RideRequestId;
            return dtoNote;
        }

        public void NoteSeen(int requestId)
        {
            _rideRequestNoteRepository.NoteSeen(requestId);
        }

        public void UpdateNote(RideRequestNoteDto note)
        {
            note.Seen = false;
            var dtoNote = GetNoteByRideRequest(note.RideRequestId);
            if (dtoNote != null)
            {
                _rideRequestNoteRepository.UpdateNote(_mapper.Map<RideRequestNoteDto, RideRequestNote>(note));
            }
            else
            {
                _rideRequestNoteRepository.AddNote(_mapper.Map<RideRequestNoteDto, RideRequestNote>(note));
            }
        }
    }
}
