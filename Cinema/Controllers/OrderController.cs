using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Cinema.Services.Interface;
using GemBox.Document;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Paragraph = iTextSharp.text.Paragraph;

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
            ComponentInfo.SetLicense("FREE-LIMITED-KEY");
            ComponentInfo.FreeLimitReached 
                += (sender, e) => e.FreeLimitReachedAction = FreeLimitReachedAction.Stop;
        }
        [Authorize]
        public IActionResult Index()
        {
            string customerId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var currentCinemaUser = this._cinemaUserService.GetDetailsForCinemaUser(customerId);

            // Retrieve a list of orders for the current customer, including related entities
            // var allOrders = _orderService.GetAllOrders();
            
            var orders= this. _orderService.GetAllOrdersForUser(customerId);
            foreach (var o in orders)
            {
                o.CinemaUser = currentCinemaUser;
            }
            

            return View(orders);
        }
        
        //todo: error
        public FileContentResult GeneratePdf()
        {
            string customerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var currentCinemaUser = this._cinemaUserService.GetDetailsForCinemaUser(customerId);
            var orders= this. _orderService.GetAllOrdersForUser(customerId);

            var templatePath = Path.Combine(Directory.GetCurrentDirectory(), "Invoice.docx");
            var document = DocumentModel.Load(templatePath);
            
            document.Content.Replace("{{UserName}}", currentCinemaUser.Name + " " + currentCinemaUser.Surname);

            StringBuilder sb = new StringBuilder();
            var totalPrice = 0.0;
            foreach (var order in orders)
            {
                totalPrice += order.TotalPrice;
                sb.Append($"Movie: {order.MovieName}\nBooking date: {order.BookingDate}\nSeats: {string.Join(", ", order.Seats)}\nPrice: {order.TotalPrice}$\n\n");
            }

            
            document.Content.Replace("{{MovieList}}", sb.ToString());
            document.Content.Replace("{{TotalPrice}}", totalPrice.ToString()+"$");

            var stream = new MemoryStream();
            document.Save(stream, new PdfSaveOptions());
            
            return File(stream.ToArray(), 
                new PdfSaveOptions().ContentType, 
                "ExportedInvoice.pdf");
            
        }
    }
}