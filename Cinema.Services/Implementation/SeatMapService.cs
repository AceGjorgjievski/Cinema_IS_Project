using System;
using System.Collections.Generic;
using System.Linq;
using Cinema.Models.Domain;
using Cinema.Repository.Interface;
using Cinema.Services.Interface;

namespace Cinema.Services.Implementation
{
    public class SeatMapService : ISeatMapService
    {
        private readonly IRepository<SeatMap> _seatMapRepository;
        
        public SeatMapService(IRepository<SeatMap> seatMapRepository)
        {
            _seatMapRepository = seatMapRepository;
        }

        public List<SeatMap> GetAllSeatMaps()
        {
            return this._seatMapRepository.GetAll().ToList();
        }

        public SeatMap GetDetailsForSeatMap(Guid? id)
        {
            return this._seatMapRepository.Get(id);
        }

        public void CreateNewSeatMap(SeatMap seatMap)
        {
            this._seatMapRepository.Insert(seatMap);
        }

        public void UpdateExistingSeatMap(SeatMap seatMap)
        {
            this._seatMapRepository.Update(seatMap);
        }

        public void DeleteSeatMap(Guid id)
        {
            SeatMap seatMap = this.GetDetailsForSeatMap(id);
            this._seatMapRepository.Delete(seatMap);
        }
    }
}