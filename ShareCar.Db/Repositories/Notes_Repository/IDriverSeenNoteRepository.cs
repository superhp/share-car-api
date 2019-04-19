using ShareCar.Db.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShareCar.Db.Repositories.Notes_Repository
{
    public interface IDriverSeenNoteRepository
    {
        void AddNote(DriverSeenNote note);
        IEnumerable<DriverSeenNote> GetNotesByPassenger(string email);
        void NoteSeen(int requestIds);
        void NoteUnseen(int driverNoteId);

    }
}
