using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using ShareCar.Db.Entities;
using ShareCar.Db.Repositories.Notes_Repository;
using ShareCar.Dto;

namespace ShareCar.Logic.Note_Logic
{
    public class DriverNoteLogic : IDriverNoteLogic
    {
        private readonly IDriverNoteRepository _driverNoteRepository;
        private readonly IDriverSeenNoteRepository _driverSeenNoteRepository;
        private readonly IMapper _mapper;

        public DriverNoteLogic(IDriverNoteRepository driverNoteRepository, IDriverSeenNoteRepository driverSeenNoteRepository, IMapper mapper)
        {
            _driverSeenNoteRepository = driverSeenNoteRepository;
            _driverNoteRepository = driverNoteRepository;
            _mapper = mapper;
        }

        public List<DriverNoteDto> GetNotesByDriver(string email)
        {
            var driverNotes = _driverNoteRepository.GetNotesByDriver(email).ToList();
            return _mapper.Map<List<DriverNote>, List<DriverNoteDto>>(driverNotes);
        }

        public DriverNoteDto AddNote(DriverNoteDto note)
        {
            var entity = _driverNoteRepository.AddNote(_mapper.Map<DriverNoteDto, DriverNote>(note));
            return _mapper.Map<DriverNote, DriverNoteDto>(entity);
        }

        public void UpdateNote(DriverNoteDto note)
        {
                var entity = _driverNoteRepository.UpdateNote(_mapper.Map<DriverNoteDto, DriverNote>(note));
                _driverSeenNoteRepository.NoteUnseen(entity.DriverNoteId);
        }

        public DriverNoteDto GetNoteByRide(int rideId)
        {
            var entity = _driverNoteRepository.GetNoteByRide(rideId);
            return _mapper.Map<DriverNote, DriverNoteDto>(entity);
        }
    }
}
