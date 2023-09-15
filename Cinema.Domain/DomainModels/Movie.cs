using System;
using System.Collections.Generic;

namespace Cinema.Models.Domain
{
    public class Movie : BaseEntity
    {
        // public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public string Director { get; set; }
        public string Genre { get; set; }
        public int Duration { get; set; }
        public float Price { get; set; }
    }
}