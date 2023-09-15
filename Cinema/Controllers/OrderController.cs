using System;
using System.IO;
using System.Linq;
using System.Security.Claims;
using Cinema.Services.Interface;
using iTextSharp.text;
using iTextSharp.text.pdf;
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
        
        //todo: error
        public ActionResult GeneratePdf()
        {
            MemoryStream memoryStream = new MemoryStream();
            Document document = new Document();

            document.SetMargins(36, 36, 36, 36);
            document.SetPageSize(PageSize.A4);

            try
            {
                PdfWriter writer = PdfWriter.GetInstance(document, memoryStream);
                document.Open();

                Paragraph paragraph = new Paragraph("Hello, this is your PDF content.");
                document.Add(paragraph);

                document.Close();
                memoryStream.Position = 0;

                return File(memoryStream, "application/pdf", "orders.pdf");
            }
            catch (Exception ex)
            {
                // Log the exception details
                Console.WriteLine("Error generating PDF: " + ex.Message);
                return Content("Error generating PDF");
            }
        }





    }
}