using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cinema.Models.Domain;
using Cinema.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Cinema.Controllers
{
    public class MoviesController : Controller
    {
        private readonly IMovieService _movieService;
        private readonly ISeatService _seatService;
        private readonly ISeatMapService _seatMapService;
        private readonly ICinemaUserService _cinemaUserService;
        private readonly IOrderService _orderService;
        private readonly IDateTimeKeyService _dateTimeKeyService;
        private readonly IHomeService _homeService;
        
        
        public MoviesController(IMovieService movieService, 
            ISeatService seatService, ICinemaUserService cinemaUserService, 
            IOrderService orderService, IDateTimeKeyService dateTimeKeyService,
            ISeatMapService seatMapService, IHomeService homeService)
        {
            _movieService = movieService;
            _seatService = seatService;
            _cinemaUserService = cinemaUserService;
            _orderService = orderService;
            _dateTimeKeyService = dateTimeKeyService;
            _seatMapService = seatMapService;
            _homeService = homeService;
        }

        public IActionResult Index()
        {
            DateTime today = DateTime.Now.Date;
            DateTime[] targetDates = { today, today.AddDays(1), today.AddDays(2), today.AddDays(3) };
            string[] times = { "09:00", "12:00", "15:00", "18:00" };
            var allMovies = this._movieService.GetAllMovies();
            this._homeService.AddSeatMaps(targetDates, times);

            return View(allMovies);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(
            [Bind("Id, Name, Description, Image, Director, Genre, Duration, Price")]
            Movie movie)
        {
            if (ModelState.IsValid)
            {
                movie.Id = new Guid();
                this._movieService.CreateNewMovie(movie);
                return RedirectToAction("Index", "Movies");
            }

            return View(movie);
        }


        [HttpGet]
        public IActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = _movieService.GetDetailsForMovie(id);
            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }

        [HttpPost]
        public IActionResult Edit(Guid id,
            [Bind("Id, Name, Director, Image, Description, Genre, Price, Duration")]
            Movie movie)
        {
            if (!id.Equals(movie.Id))
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    this._movieService.UpdateExistingMovie(movie);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if(!MovieExists(movie.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(movie);
        }

        private bool MovieExists(Guid movieId)
        {
            return this._movieService.GetDetailsForMovie(movieId) != null;
        }

        [HttpGet]
        public IActionResult CheckSeats(Guid? MovieId, string? time, string? date)
        {
            // var movie = await _context.Movies.Where(z => z.Id.Equals(MovieId)).FirstOrDefaultAsync();
            // MovieBookDto movieBookDto = new MovieBookDto(movie, time, date);
            //
            //
            // movieBookDto.Id = MovieId;
            // movieBookDto = this.SetDateAndTime(movieBookDto, time, date);
            // movieBookDto = this.AddSeatMapAndSeats(movieBookDto);
            
            
            var movie2 = this._movieService.GetDetailsForMovie(MovieId);
            var movieDto = _movieService.GetMovieBookDto(movie2, time, date);
            
            _movieService.MakeSeatsAvailableAgain(MovieId, time, date);
            
            
            
            
            return View(movieDto);
        }

        [HttpPost]
        public IActionResult SeatOrder(Guid? Id, string[] seatIds, string selectedDate, string selectedTime)
        {
            if (ModelState.IsValid)
            {
                var seatGuids = seatIds.Select(Guid.Parse).ToList();

                // Retrieve the seats based on their IDs
                // var seats = await _context.Seats.Where(z => seatGuids.Contains(z.Id)).ToListAsync();
                var seats = this._seatService.GetAllSeats().Where(z => seatGuids.Contains(z.Id)).ToList();
                // Get the current user
                // string customerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                // Guid customerGuid = Guid.Parse(customerId);

                // Create a new Order
                // var movie = _context.Movies.FirstOrDefault(z => z.Id.Equals(Id));
                var movie = this._movieService.GetDetailsForMovie(Id);
                // var currentCustomer = _context.CinemaUsers.FirstOrDefault(u => u.Id == customerGuid);

                var cinemaUser = this._cinemaUserService.GetCurrentUser(User);

                // Create a new Order and set its properties
                var currentOrder = new Order
                {
                    Id=Guid.NewGuid(),
                    MovieName = movie.Name,
                    TotalPrice = seats.Sum(z => z.SeatPrice),
                    BookingDate = selectedDate,
                    BookingTime = selectedTime,
                    CinemaUser = cinemaUser,
                    Seats = seats.Select(s => s.SeatNumber).ToList(),
                    CinemaUserId = cinemaUser.Id
                };
                
                currentOrder = _seatService.UpdateSeatAvailability(currentOrder, seats, false);
                
                cinemaUser.Orders = new List<Order>();
                cinemaUser.Orders.Add(currentOrder);

                // currentCustomer.Orders = new List<Order>();
                // currentCustomer.Orders.Add(currentOrder);
                
                

                
                // var dateTimeKey = _context.DateTimeKeys.FirstOrDefault(z => z.Date.Equals(selectedDate) && z.Time.Equals(selectedTime));
                var dateTimeKey = this._dateTimeKeyService.GetAllDateTimeKeys()
                    .FirstOrDefault(z => z.Date.Equals(selectedDate) && z.Time.Equals(selectedTime));
                
                
                // var seatMap = _context.SeatMaps.FirstOrDefault(z => z.DateTimeKey.Equals(dateTimeKey));
                var seatMap = this._seatMapService.GetAllSeatMaps()
                    .FirstOrDefault(z => z.DateTimeKey.Equals(dateTimeKey));
                seatMap.Movie = movie;
                seatMap.CinemaUser = cinemaUser;
                this._seatMapService.UpdateExistingSeatMap(seatMap);

                // Add the new order to the context
                this._orderService.CreateNewOrder(currentOrder);
                // _context.Orders.Add(currentOrder);

                // Save changes to the database
                // await _context.SaveChangesAsync();

                return RedirectToAction("Index", "Movies");
            }

            return null;
        }
    }
}