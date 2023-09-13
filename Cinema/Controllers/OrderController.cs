using System;
using System.Linq;
using System.Security.Claims;
using Cinema.Data;
using Cinema.Models.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Cinema.Controllers
{
    public class OrderController : Controller
    {
        private readonly ApplicationDbContext _context;
        
        public OrderController(ApplicationDbContext context)
        {
            _context = context;
        }
        [Authorize]
        public IActionResult Index()
        {
            string customerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            Guid customerGuid = Guid.Parse(customerId);

            // Retrieve a list of orders for the current customer, including related entities
            var orders = _context.Orders
                .Include(c => c.CinemaUser)
                .ToList();

            return View(orders);
        }



    }
}