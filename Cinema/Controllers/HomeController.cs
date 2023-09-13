﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Cinema.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Cinema.Models;
using Cinema.Models.Domain;

namespace Cinema.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly ApplicationDbContext _context;


        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            // Get the current date
            DateTime today = DateTime.Now.Date;
            // Calculate dates for today, tomorrow, and the day after tomorrow
            DateTime[] targetDates = { today, today.AddDays(1), today.AddDays(2), today.AddDays(3) };
            string[] times = { "09:00", "12:00", "15:00", "18:00" };
            DateTime now = DateTime.Now;
            string todayString = now.ToShortDateString();
            string currentTime = now.ToString("HH:mm");
            var movies = _context.Movies.ToList();
            
            this.DeleteDateTimeKeysAndRelatedSeatMaps(now, currentTime);

            this.addDateTimeKeys(targetDates, times);
            
            this.AddSeatMaps(targetDates, times, movies);



            return View();
        }

        private void AddSeatMaps(DateTime[] targetDates, string[] times, List<Movie> movies)
        {
            foreach (var targetDate in targetDates)
            {
                string formattedDate = targetDate.ToString("yyyy-MM-dd");
                for (int i = 0; i < times.Length; i++)
                {
                    var time = times[i];
                    var dateTimeKey = _context.DateTimeKeys
                        .FirstOrDefault(key => key.Date.Equals(formattedDate) && key.Time.Equals(time));


                    if (dateTimeKey != null)
                    {
                        //checking for seatMaps with the same date and time
                        //ako ne postoi takov seatMap, go kreirame
                        foreach(var movie in movies)
                        {
                            var movieId = movie.Id;
                            
                            if (!_context.SeatMaps.Any(sm => sm.Movie.Id.Equals(movieId) && sm.DateTimeKey.Id.Equals(dateTimeKey.Id)))
                            {

                                SeatMap seatMap = new SeatMap()
                                {
                                    DateTimeKey = dateTimeKey,
                                    Movie = movie,
                                    Seats = Enumerable.Range(1, 20).Select(seatNum => new Seat()
                                        {
                                            SeatNumber = seatNum,
                                            IsAvailable = true,
                                            DateTimeKey = dateTimeKey,
                                            SeatPrice = 10,
                                            Movie = movie
                                        }).OrderBy(seat => seat.SeatNumber)
                                        .ToList()
                                };
                                _context.SeatMaps.Add(seatMap);
                                _context.SaveChanges();
                            }
                            //checking if there no seats but there are seatMaps with the same date and time
                    
                            // if (!_context.Seats.Any(s => s.DateTimeKey.Id.Equals(dateTimeKey) || s.Movie.Id.Equals(moiveId)))
                            // {
                            //     for (int j = 1; j <= 20; j++)
                            //     {
                            //         Seat seat = new Seat()
                            //         {
                            //             SeatNumber = j,
                            //             IsAvailable = true,
                            //             DateTimeKey = dateTimeKey,
                            //             SeatPrice = 10
                            //         };
                            //         _context.Seats.Add(seat);
                            //         _context.SaveChanges();
                            //
                            //     }
                            // }
                        }
                        
                        
                    }

                }

            }

        }

        private void addDateTimeKeys(DateTime[] targetDates, string[] times)
        {
            foreach (var targetDate in targetDates)
            {
                string formattedDate = targetDate.ToString("yyyy-MM-dd");

                // Check if the date is not already in the database
                if (!_context.DateTimeKeys.Any(key => key.Date == formattedDate))
                {
                    // Add the date to the database
                    for (int i = 9; i <= 18; i += 3)
                    {
                        DateTimeKey dateTimeKey = new DateTimeKey
                        {
                            Date = formattedDate,
                            Time = $"{i:00}:00"
                        };

                        _context.DateTimeKeys.Add(dateTimeKey);
                    }
                    //Check if the date is in the db but the time is missing
                } else if (_context.DateTimeKeys.Any(key => key.Date == formattedDate))
                {
                    foreach (var time in times)
                    {
                        if (!_context.DateTimeKeys.Any(key => key.Time.Equals(time)))
                        {
                            DateTimeKey dateTimeKey = new DateTimeKey
                            {
                                Date = formattedDate,
                                Time = time
                            };
                            _context.DateTimeKeys.Add(dateTimeKey);
                        }
                    }
                }
            }


            _context.SaveChanges();
        }


        private void DeleteDateTimeKeysAndRelatedSeatMaps(DateTime now, string currentTime)
        {
            // Date is today, and time is before the current time
            var dateTimeKeysToDelete = _context.DateTimeKeys.ToList()
                .Where(key =>
                    DateTime.Parse(key.Date) < now.Date || // Date is before today
                    (DateTime.Parse(key.Date).Equals(now.Date) && (key.Time.CompareTo(currentTime) < 0))) 
                .ToList();
            
            foreach (var keyToDelete in dateTimeKeysToDelete)
            {
                var existingKey = _context.DateTimeKeys.Find(keyToDelete.Id);
                if (existingKey != null)
                {
                    var seatsToDelete = _context.Seats
                        .Where(s => s.DateTimeKey == null || s.DateTimeKey.Id.Equals(keyToDelete.Id))
                        .ToList();

                    foreach (var seat in seatsToDelete)
                    {
                        _context.Seats.Remove(seat);
                    }
                    
                    // Find and remove related SeatMaps with null DateTimeKey reference
                    var seatMapsToDelete = _context.SeatMaps
                        .Where(sm => sm.DateTimeKey == null || sm.DateTimeKey.Id.Equals(keyToDelete.Id))
                        .ToList();

                    foreach (var seatMapToDelete in seatMapsToDelete)
                    {
                        _context.SeatMaps.Remove(seatMapToDelete);
                    }
                    
                    _context.DateTimeKeys.Remove(existingKey);

                    _context.SaveChanges();
                }
            }
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}