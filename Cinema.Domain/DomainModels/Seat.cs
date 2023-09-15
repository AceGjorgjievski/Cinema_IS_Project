using System;
using Cinema.Models.Identity;

namespace Cinema.Models.Domain
{
    public class Seat : BaseEntity
    {
        // public Guid Id { get; set; }
        public int SeatNumber { get; set; }
        public int SeatPrice { get; set; }
        public bool IsAvailable { get; set; }
        
        public DateTime Date { get; set; }
        public string Time { get; set; }
        public DateTimeKey DateTimeKey { get; set; }
        public Movie Movie { get; set; }
    }
}