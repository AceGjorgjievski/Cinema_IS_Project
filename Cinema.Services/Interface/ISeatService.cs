using System;
using System.Collections.Generic;
using Cinema.Models.Domain;

namespace Cinema.Services.Interface
{
    public interface ISeatService
    {
        List<Seat> GetAllSeats();
        Seat GetDetailsForSeat(Guid? id);
        void CreateNewSeat(Seat s);
        void UpdateExistingSeat(Seat s);
        void DeleteSeat(Guid id);
        Order UpdateSeatAvailability(Order currentOrder, List<Seat> seats, bool setAvailability);
    }
}