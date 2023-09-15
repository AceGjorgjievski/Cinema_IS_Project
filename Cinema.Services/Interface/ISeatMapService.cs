using System;
using System.Collections.Generic;
using Cinema.Models.Domain;

namespace Cinema.Services.Interface
{
    public interface ISeatMapService
    {
        List<SeatMap> GetAllSeatMaps();
        SeatMap GetDetailsForSeatMap(Guid? id);
        void CreateNewSeatMap(SeatMap seatMap);
        void UpdateExistingSeatMap(SeatMap seatMap);
        void DeleteSeatMap(Guid id);
    }
}