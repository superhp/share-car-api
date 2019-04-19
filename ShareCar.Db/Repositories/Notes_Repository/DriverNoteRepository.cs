using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;
using ShareCar.Db.Entities;

namespace ShareCar.Db.Repositories.Notes_Repository
{
    public class DriverNoteRepository : IDriverNoteRepository
    {
        private readonly ApplicationDbContext _databaseContext;

        public DriverNoteRepository(ApplicationDbContext databaseContext)
        {
            _databaseContext = databaseContext;
        }
        public DriverNote AddNote(DriverNote note)
        {
            var entity = _databaseContext.Add(note).Entity;
            _databaseContext.SaveChanges();
            return entity;
        }

        public DriverNote GetNoteByRide(int rideId)
        {
            return _databaseContext.DriverNotes.Include(x => x.Ride).FirstOrDefault(x => x.Ride.RideId == rideId);
        }

        public IEnumerable<DriverNote> GetNotesByDriver(string email)
        {
            return _databaseContext.DriverNotes.Include(x => x.Ride).Where(x => x.Ride.DriverEmail == email);
        }

        public DriverNote UpdateNote(DriverNote note)
        {
            var entity = _databaseContext.DriverNotes.Include(x => x.Ride).Single(x => x.Ride.RideId == note.RideId);
            entity.Text = note.Text;
            _databaseContext.Update(entity);
            _databaseContext.SaveChanges();
            return entity;
        }
    }
}
