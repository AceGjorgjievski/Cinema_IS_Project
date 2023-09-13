using System;
using System.Collections.Generic;
using Cinema.Models.Identity;

namespace Cinema.Models.Domain
{
    public class SeatMap
    {
        public Guid Id { get; set; }
        public DateTimeKey DateTimeKey { get; set; }
        public List<Seat> Seats { get; set; }
        
        public Movie Movie { get; set; }
        public CinemaUser CinemaUser { get; set; }

        public SeatMap()
        {
            
        }
    }
}