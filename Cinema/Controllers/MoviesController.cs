using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Cinema.Data;
using Cinema.Models.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.JSInterop.Infrastructure;

namespace Cinema.Controllers
{
    public class MoviesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MoviesController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Movies.ToListAsync());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(
            [Bind("Id, Name, Description, Image, Director, Genre, Duration, Price")]
            Movie movie)
        {
            if (ModelState.IsValid)
            {
                movie.Id = new Guid();
                _context.Add(movie);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Movies");
            }

            return View(movie);
        }


        public IActionResult Edit()
        {
            throw new System.NotImplementedException();
        }

        [HttpGet]
        public async Task<IActionResult> CheckSeats(Guid? id, string? time, string? date)
        {
            var movie = await _context.Movies.Where(z => z.Id.Equals(id)).FirstOrDefaultAsync();
            MovieBookDto movieBookDto = new MovieBookDto(movie, time, date);

            
            movieBookDto.Id = id;
            movieBookDto = this.SetDateAndTime(movieBookDto, time, date);
            movieBookDto = this.AddSeatMapAndSeats(movieBookDto);
            
            
            
            return View(movieBookDto);
        }
        
        private MovieBookDto AddSeatMapAndSeats(MovieBookDto dto)
        {
            dto.SeatMap = _context.SeatMaps
                .FirstOrDefault(z => z.DateTimeKey.Date.Equals(dto.SelectedDate) && 
                                     z.DateTimeKey.Time.Equals(dto.SelectedTime) &&
                                     z.Movie.Name.Equals(dto.MovieName));

            dto.SeatMap.Seats = _context.Seats
                .Where(z => z.DateTimeKey.Date.Equals(dto.SelectedDate) && 
                            z.DateTimeKey.Time.Equals(dto.SelectedTime) &&
                            z.Movie.Name.Equals(dto.MovieName))
                .OrderBy(z => z.SeatNumber)
                .ToList();

            return dto;
        }

        private MovieBookDto SetDateAndTime(MovieBookDto dto, string? time, string? date)
        {
            if (string.IsNullOrEmpty(time) && string.IsNullOrEmpty(date))
            {
                // Default date and time
                var defaultDate = DateTime.Now.Date;
                var defaultTime = TimeSpan.Parse("09:00");
        
                // Check if any of the times (9:00, 12:00, 15:00, 18:00) have passed
                var currentTime = DateTime.Now.TimeOfDay;
                var timesToCheck = new TimeSpan[] { TimeSpan.Parse("09:00"), TimeSpan.Parse("12:00"), TimeSpan.Parse("15:00"), TimeSpan.Parse("18:00") };
                bool anyTimePassed = timesToCheck.Any(t => currentTime > t);

                // If none of the times have passed, use the default date and time
                if (!anyTimePassed)
                {
                    dto.SelectedDate = defaultDate.ToString("yyyy-MM-dd");
                    dto.SelectedTime = defaultTime.ToString("hh\\:mm");
                }
                else
                {
                    // If any of the times have passed, find the closest upcoming time
                    var upcomingTimes = timesToCheck.Where(t => t > currentTime).OrderBy(t => t).ToList();

                    if (upcomingTimes.Any())
                    {
                        // Use the closest upcoming time
                        var closestTime = upcomingTimes.First();
                        dto.SelectedTime = closestTime.ToString("hh\\:mm");
                        dto.SelectedDate = defaultDate.ToString("yyyy-MM-dd");
                    }
                    else
                    {
                        // If all times have passed, use the next day's date and the earliest time (9:00)
                        dto.SelectedDate = defaultDate.AddDays(1).ToString("yyyy-MM-dd");
                        dto.SelectedTime = "09:00";
                        dto.MinDate = defaultDate.AddDays(1);
                    }
                }
            }
            else
            {
                dto.SelectedTime = time;
                dto.SelectedDate = date;
            }
            
            return dto;
        }

        [HttpPost]
        public async Task<IActionResult> SeatOrder(Guid? Id, string[] seatIds, string selectedDate, string selectedTime)
        {
            if (ModelState.IsValid)
            {
                var seatGuids = seatIds.Select(Guid.Parse).ToList();

                // Retrieve the seats based on their IDs
                var seats = await _context.Seats.Where(z => seatGuids.Contains(z.Id)).ToListAsync();

                // Get the current user
                string customerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                Guid customerGuid = Guid.Parse(customerId);

                // Create a new Order
                var movie = _context.Movies.FirstOrDefault(z => z.Id.Equals(Id));
                var currentCustomer = _context.CinemaUsers.FirstOrDefault(u => u.Id == customerGuid);

                // Create a new Order and set its properties
                var currentOrder = new Order
                {
                    MovieName = movie.Name,
                    TotalPrice = seats.Sum(z => z.SeatPrice),
                    BookingDate = selectedDate,
                    BookingTime = selectedTime,
                    CinemaUser = currentCustomer,
                    Seats = new List<int>()
                };
                
                

                // Mark the seats as unavailable
                foreach (var seat in seats)
                {
                    seat.IsAvailable = false;
                    currentOrder.Seats.Add(seat.SeatNumber);
                }

                currentCustomer.Orders = new List<Order>();
                currentCustomer.Orders.Add(currentOrder);

                
                var dateTimeKey = _context.DateTimeKeys.FirstOrDefault(z => z.Date.Equals(selectedDate) && z.Time.Equals(selectedTime));

                var seatMap = _context.SeatMaps.FirstOrDefault(z => z.DateTimeKey.Equals(dateTimeKey));
                seatMap.Movie = movie;
                seatMap.CinemaUser = currentCustomer;

                // Add the new order to the context
                _context.Orders.Add(currentOrder);

                // Save changes to the database
                await _context.SaveChangesAsync();

                return RedirectToAction("Index", "Movies");
            }

            return null;
        }

    }
}