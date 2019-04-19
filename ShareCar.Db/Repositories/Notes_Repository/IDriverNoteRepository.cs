using ShareCar.Db.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShareCar.Db.Repositories.Notes_Repository
{
    public interface IDriverNoteRepository
    {
        IEnumerable<DriverNote> GetNotesByDriver(string email);
        DriverNote AddNote(DriverNote note);
        DriverNote GetNoteByRide(int rideId);
        DriverNote UpdateNote(DriverNote note);
    }
}
