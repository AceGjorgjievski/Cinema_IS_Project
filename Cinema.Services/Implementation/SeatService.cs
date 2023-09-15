using System;
using System.Collections.Generic;
using System.Linq;
using Cinema.Models.Domain;
using Cinema.Repository.Interface;
using Cinema.Services.Interface;

namespace Cinema.Services.Implementation
{
    public class SeatService : ISeatService
    {
        private readonly IRepository<Seat> _seatRepository;
        
        public SeatService(IRepository<Seat> seatRepository)
        {
            _seatRepository = seatRepository;
        }
        
        public List<Seat> GetAllSeats()
        {
            return this._seatRepository.GetAll().ToList();
        }

        public Seat GetDetailsForSeat(Guid? id)
        {
            return this._seatRepository.Get(id);
        }

        public void CreateNewSeat(Seat s)
        {
            this._seatRepository.Insert(s);
        }

        public void UpdateExistingSeat(Seat s)
        {
            this._seatRepository.Update(s);
        }

        public void DeleteSeat(Guid id)
        {
            Seat s = this.GetDetailsForSeat(id);
            this._seatRepository.Delete(s);
        }

        public Order UpdateSeatAvailability(Order currentOrder, List<Seat> seats, bool setAvailability)
        {
            foreach (var seat in seats)
            {
                seat.IsAvailable = false;
            }
            return currentOrder;
        }
    }
}