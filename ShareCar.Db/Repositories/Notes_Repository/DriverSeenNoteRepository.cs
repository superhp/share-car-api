using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShareCar.Db.Entities;
using ShareCar.Db.Repositories.RideRequest_Repository;

namespace ShareCar.Db.Repositories.Notes_Repository
{
    public class DriverSeenNoteRepository : IDriverSeenNoteRepository
    {
        private readonly ApplicationDbContext _databaseContext;
        private readonly IRideRequestRepository _rideRequestRepository;

        public DriverSeenNoteRepository(ApplicationDbContext databaseContext, IRideRequestRepository rideRequestRepository)
        {
            _databaseContext = databaseContext;
            _rideRequestRepository = rideRequestRepository;
        }

        public void AddNote(DriverSeenNote note)
        {
            _databaseContext.Add(note);
            _databaseContext.SaveChanges();
        }

        public IEnumerable<DriverSeenNote> GetNotesByPassenger(string email)
        {
            var requests = _databaseContext.Requests.Where(x => x.PassengerEmail == email && (x.SeenByPassenger == false || x.Status == Status.ACCEPTED || x.Status == Status.WAITING)).ToList();
            return _databaseContext.DriverSeenNotes.Where(x => requests.FirstOrDefault(y => y.RideRequestId == x.RideRequestId) != null);
        }

        public void NoteSeen(int requestId)
        {
            var note = _databaseContext.DriverSeenNotes.Single(x => x.RideRequestId == requestId);
            note.Seen = true;
            _databaseContext.SaveChanges();
        }

        public void NoteUnseen(int driverNoteId)
        {
            var notes = _databaseContext.DriverSeenNotes.Where(x => x.DriverNoteId == driverNoteId);
            foreach(var note in notes)
            {
                note.Seen = false;
            }
            _databaseContext.SaveChanges();
        }
    }
}
