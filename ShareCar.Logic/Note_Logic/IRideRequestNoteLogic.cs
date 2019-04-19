using ShareCar.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShareCar.Logic.Note_Logic
{
    public interface IRideRequestNoteLogic
    {
        void AddNote(RideRequestNoteDto note);
        RideRequestNoteDto GetNoteByRide(int rideId);
        RideRequestNoteDto GetNoteByRideRequest(int rideRequestId);
        List<RideRequestNoteDto> GetNoteByPassenger(string email);
        void UpdateNote(RideRequestNoteDto note);
        void NoteSeen(int requestId);
    }
}
