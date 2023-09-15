using System;
using System.Linq;
using System.Security.Claims;
using Cinema.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cinema.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly ICinemaUserService _cinemaUserService;
        
        public OrderController(IOrderService orderService,
            ICinemaUserService cinemaUserService)
        {
            _orderService = orderService;
            _cinemaUserService = cinemaUserService;
        }
        [Authorize]
        public IActionResult Index()
        {
            string customerId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var currentCinemaUser = this._cinemaUserService.GetDetailsForCinemaUser(customerId);

            // Retrieve a list of orders for the current customer, including related entities
            var allOrders = _orderService.GetAllOrders();

            // var orders = _orderService.GetAllOrders()
            //     .ToList().Where(z => z.CinemaUser.Id.Equals(customerId)).ToList();

            return View(allOrders);
        }



    }
}