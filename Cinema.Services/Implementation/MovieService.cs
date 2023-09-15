using System;
using System.Collections.Generic;
using System.Linq;
using Cinema.Models.Domain;
using Cinema.Repository.Interface;
using Cinema.Services.Interface;

namespace Cinema.Services.Implementation
{
    public class MovieService : IMovieService
    {
        private readonly IRepository<Movie> _movieRepository;
        private readonly IRepository<Seat> _seatRepository;
        private readonly IRepository<SeatMap> _seatMapRepository;
        private readonly IRepository<DateTimeKey> _dateTimeKeyRepository;
        private readonly IRepository<Order> _orderRepository;

        public MovieService(IRepository<Movie> movieRepository, 
            IRepository<Seat> seatRepository,IRepository<SeatMap> seatMapRepository,
            IRepository<DateTimeKey> dateTimeKeyRepository, IRepository<Order> orderRepository)
        {
            _movieRepository = movieRepository;
            _seatMapRepository = seatMapRepository;
            _seatRepository = seatRepository;
            _dateTimeKeyRepository = dateTimeKeyRepository;
            _orderRepository = orderRepository;
        }

        public List<Movie> GetAllMovies()
        {
            return this._movieRepository.GetAll().ToList();
        }

        public Movie GetDetailsForMovie(Guid? id)
        {
            return this._movieRepository.Get(id);
        }

        public void CreateNewMovie(Movie m)
        {
            this._movieRepository.Insert(m);
        }

        public void UpdateExistingMovie(Movie m)
        {
            this._movieRepository.Update(m);
        }

        public void DeleteMovie(Guid id)
        {
            Movie m = this.GetDetailsForMovie(id);
            this._movieRepository.Delete(m);
        }

        public MovieBookDto GetMovieBookDto(Movie movie, string time, string date)
        {
            MovieBookDto movieBookDto = new MovieBookDto(movie, time, date);
            movieBookDto.Id = movie.Id;
            movieBookDto = this.SetDateAndTime(movieBookDto, time, date);
            movieBookDto = this.AddSeatMapAndSeats(movieBookDto);

            return movieBookDto;
        }

        public void MakeSeatsAvailableAgain(Guid? movieId, string time, string date)
        {
            if (String.IsNullOrEmpty(time) && String.IsNullOrEmpty(date))
            {
                var defaultDate = DateTime.Now.Date;
                var defaultTime = TimeSpan.Parse("09:00");

                // Check if any of the times (9:00, 12:00, 15:00, 18:00) have passed
                var currentTime = DateTime.Now.TimeOfDay;
                var timesToCheck = new TimeSpan[]
                {
                    TimeSpan.Parse("09:00"), TimeSpan.Parse("12:00"), TimeSpan.Parse("15:00"), TimeSpan.Parse("18:00")
                };
                bool anyTimePassed = timesToCheck.Any(t => currentTime > t);

                // If none of the times have passed, use the default date and time
                if (!anyTimePassed)
                {
                    date = defaultDate.ToString("yyyy-MM-dd");
                    time = defaultTime.ToString("hh\\:mm");
                }
                else
                {
                    // If any of the times have passed, find the closest upcoming time
                    var upcomingTimes = timesToCheck.Where(t => t > currentTime).OrderBy(t => t).ToList();

                    if (upcomingTimes.Any())
                    {
                        // Use the closest upcoming time
                        var closestTime = upcomingTimes.First();
                        time = closestTime.ToString("hh\\:mm");
                        date = defaultDate.ToString("yyyy-MM-dd");
                    }
                    else
                    {
                        // If all times have passed, use the next day's date and the earliest time (9:00)
                        date = defaultDate.AddDays(1).ToString("yyyy-MM-dd");
                        time = "09:00";
                    }
                }
            }
            
            var selectedMovie = this._movieRepository.Get(movieId);
            var unavailableSeats = this._seatRepository.GetAll()
                .Where(z => z.IsAvailable == false)
                .ToList();
            

            var orders = this._orderRepository.GetAll().ToList();

            var seatsNotInOrders = unavailableSeats
                .Where(seat => !orders.Any(order => order.Seats.Contains(seat.SeatNumber)))
                .ToList();

            foreach (var seat in seatsNotInOrders)
            {
                seat.IsAvailable = true;
                this._seatRepository.Update(seat);
            }
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
                var timesToCheck = new TimeSpan[]
                {
                    TimeSpan.Parse("09:00"), TimeSpan.Parse("12:00"), TimeSpan.Parse("15:00"), TimeSpan.Parse("18:00")
                };
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
                dto.DateTimeKey = _dateTimeKeyRepository.GetAll()
                    .FirstOrDefault(z => z.Date.Equals(dto.SelectedDate) && z.Time.Equals(dto.SelectedTime));
            }
            else
            {
                
                dto.SelectedTime = time;
                dto.SelectedDate = date;
                dto.DateTimeKey = _dateTimeKeyRepository.GetAll()
                    .FirstOrDefault(z => z.Date.Equals(date) && z.Time.Equals(time));
            }

            var currentTime2 = DateTime.Now.TimeOfDay;
            var timesToCheck2 = new List<TimeSpan> { TimeSpan.Parse("09:00"), TimeSpan.Parse("12:00"), TimeSpan.Parse("15:00"), TimeSpan.Parse("18:00") };

            
            
            if(DateTime.Parse(dto.SelectedDate) > DateTime.Now.Date &&
               currentTime2 > TimeSpan.Parse(time))
            {
                dto.MinDate = DateTime.Now.Date.AddDays(1);
            }
            else
            {
                dto.MinDate = DateTime.Now.Date;
            }
            

            return dto;
        }
        
        private MovieBookDto AddSeatMapAndSeats(MovieBookDto dto)
        {
            dto.SeatMap =  this._seatMapRepository.GetAll()
                .ToList()
                .FirstOrDefault(z => z.DateTimeKey != null &&
                                     z.DateTimeKey.Id.Equals(dto.DateTimeKey.Id) && 
                                     z.Movie != null &&
                                     z.Movie.Id.Equals(dto.Movie.Id));

            
            dto.SeatMap.Seats =  this._seatRepository.GetAll()
                .ToList()
                .Where(z => z.DateTimeKey != null &&
                            z.DateTimeKey.Id.Equals(dto.DateTimeKey.Id) &&
                            z.Movie != null &&
                            z.Movie.Id.Equals(dto.Movie.Id))
                .OrderBy(z => z.SeatNumber)
                .ToList();
            return dto;
        }
    }
}