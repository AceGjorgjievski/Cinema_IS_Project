using System;
using System.Collections.Generic;
using Cinema.Models.Domain;
using Microsoft.AspNetCore.Identity;

namespace Cinema.Models.Identity
{
    public class CinemaUser : IdentityUser<Guid>
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Address { get; set; }
        public ICollection<Order> Orders { get; set; }
        
    }
}