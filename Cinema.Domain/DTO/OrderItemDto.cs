using System.Collections.Generic;
using Cinema.Models.Identity;

namespace Cinema.Models.Domain
{
    public class OrderItemDto
    {
        public CinemaUser CinemaUser { get; set; }
        public Movie Movie { get; set; }
        public List<Seat> Seats { get; set; }
        public float TotalPrice { get; set; }
        public string BookingDate { get; set; }
        public string BookingTime { get; set; }
    }
}