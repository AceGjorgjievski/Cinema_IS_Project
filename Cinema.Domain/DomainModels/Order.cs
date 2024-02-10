using System;
using System.Collections.Generic;
using Cinema.Models.Identity;

namespace Cinema.Models.Domain
{
    public class Order : BaseEntity
    {
        // public Guid Id { get; set; }
        public string MovieName { get; set; }
        public List<int> Seats { get; set; }
        public float TotalPrice { get; set; }
        public string BookingDate { get; set; }
        public string BookingTime { get; set; }
        
        public CinemaUser CinemaUser { get; set; }
        public string CinemaUserId { get; set; }

        public Order()
        {
            
        }
    }
}