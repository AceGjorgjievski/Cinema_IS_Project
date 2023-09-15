using System;
using System.Collections.Generic;
using System.Linq;
using Cinema.Models.Domain;
using Cinema.Repository.Interface;
using Cinema.Services.Interface;

namespace Cinema.Services.Implementation
{
    public class HomeService : IHomeService
    {
        private readonly IRepository<DateTimeKey> _dateTimeKeyRepository;
        private readonly IRepository<Seat> _seatRepository;
        private readonly IRepository<SeatMap> _seatMapRepository;
        private readonly IRepository<Movie> _movieRepository;

        public HomeService(IRepository<DateTimeKey> dateTimeKeyRepository,
            IRepository<Seat> seatRepository, IRepository<SeatMap> seatMapRepository,
            IRepository<Movie> movieRepository)
        {
            _dateTimeKeyRepository = dateTimeKeyRepository;
            _seatMapRepository = seatMapRepository;
            _seatRepository = seatRepository;
            _movieRepository = movieRepository;
        }


        public void DeleteDateTimeKeysAndRelatedSeatMaps(DateTime now, string currentTime)
        {
            var dateTimeKeysToDelete = this._dateTimeKeyRepository.GetAll().ToList()
                .Where(key =>
                    DateTime.Parse(key.Date) < now.Date || // Date is before today
                    (DateTime.Parse(key.Date).Equals(now.Date) && (key.Time.CompareTo(currentTime) < 0)))
                .ToList();

            HashSet<string> uniqueDateTimeKeys = new HashSet<string>();
            var allDateTimeKeys = this._dateTimeKeyRepository.GetAll().ToList();
            foreach (var key in allDateTimeKeys)
            {
                var keyStr = $"{key.Date} {key.Time}";
                if (uniqueDateTimeKeys.Contains(keyStr))
                {
                    dateTimeKeysToDelete.Add(key);
                }
                else
                {
                    uniqueDateTimeKeys.Add(keyStr);
                }
            }

            foreach (var keyToDelete in dateTimeKeysToDelete)
            {
                var existingKey = this._dateTimeKeyRepository.Get(keyToDelete.Id);
                if (existingKey != null)
                {
                    var seatsToDelete = this._seatRepository.GetAll()
                        .Where(s => s.DateTimeKey == null || s.DateTimeKey.Id.Equals(keyToDelete.Id))
                        .ToList();

                    foreach (var seat in seatsToDelete)
                    {
                        this._seatRepository.Delete(seat);
                    }

                    // Find and remove related SeatMaps with null DateTimeKey reference
                    var seatMapsToDelete = this._seatMapRepository.GetAll()
                        .Where(sm => sm.DateTimeKey == null || sm.DateTimeKey.Id.Equals(keyToDelete.Id))
                        .ToList();

                    foreach (var seatMapToDelete in seatMapsToDelete)
                    {
                        this._seatMapRepository.Delete(seatMapToDelete);
                    }

                    this._dateTimeKeyRepository.Delete(existingKey);
                }
            }
        }

        public void addDateTimeKeys(DateTime[] targetDates, string[] times)
        {
            DateTime now = DateTime.Now;
            string todayString = now.ToString("yyyy-MM-dd");
            string currentTime = now.ToString("HH:mm");

            if (_dateTimeKeyRepository.GetAll().ToList().Count == 0) //ako e prazna db
            {
                foreach (var targetDate in targetDates)
                {
                    string formattedDate = targetDate.ToString("yyyy-MM-dd");
                    foreach (var time in times)
                    {
                        // if (currentTime.CompareTo(time) < 0 && formattedDate.Equals(todayString)) //vidi dali momentalnoto vreme < times []
                        // {
                        var existingKey = _dateTimeKeyRepository.GetAll()
                            .FirstOrDefault(z => z.Date.Equals(formattedDate) && z.Time.Equals(time));
                        //ako ne postoi dateTimeKey vo db, dodaj go
                        if (existingKey == null)
                        {
                            //dodaj gi onie shto se pogolemi od momentalnoto vreme
                            DateTimeKey dateTimeKey = new DateTimeKey
                            {
                                Date = formattedDate,
                                Time = time
                            };
                            this._dateTimeKeyRepository.Insert(dateTimeKey);
                        }
                    }
                }
            }
            else if (_dateTimeKeyRepository.GetAll().ToList().Count < 16)
            {
                foreach (var targetDate in targetDates)
                {
                    string formattedDate = targetDate.ToString("yyyy-MM-dd");
                    foreach (var time in times)
                    {
                        if (currentTime.CompareTo(time) < 0) //vidi dali momentalnoto vreme < times []
                        {
                            var existingKey = _dateTimeKeyRepository.GetAll()
                                .FirstOrDefault(z => z.Date.Equals(formattedDate) && z.Time.Equals(time));
                            //ako ne postoi dateTimeKey vo db, dodaj go
                            if (existingKey == null)
                            {
                                DateTimeKey dateTimeKey = new DateTimeKey
                                {
                                    Date = formattedDate,
                                    Time = time
                                };
                                this._dateTimeKeyRepository.Insert(dateTimeKey);
                            }
                        }
                    }
                }
            }

            // foreach (var targetDate in targetDates)
            // {
            //     if (_dateTimeKeyRepository.GetAll().ToList().Count == 0) //ako e prazna db
            //     {
            //         foreach (var time in times)
            //         {
            //             // if (currentTime.CompareTo(time) < 0 && formattedDate.Equals(todayString)) //vidi dali momentalnoto vreme < times []
            //             // {
            //             var existingKey = _dateTimeKeyRepository.GetAll()
            //                 .FirstOrDefault(z => z.Date.Equals(formattedDate) && z.Time.Equals(time));
            //             //ako ne postoi dateTimeKey vo db, dodaj go
            //             if (existingKey == null)
            //             {
            //                 //dodaj gi onie shto se pogolemi od momentalnoto vreme
            //                 DateTimeKey dateTimeKey = new DateTimeKey
            //                 {
            //                     Date = formattedDate,
            //                     Time = time
            //                 };
            //                 this._dateTimeKeyRepository.Insert(dateTimeKey);
            //             }
            // }
            // else
            // {
            //     var existingKey = _dateTimeKeyRepository.GetAll()
            //         .FirstOrDefault(z => z.Date.Equals(formattedDate) && z.Time.Equals(time));
            //     //ako ne postoi dateTimeKey vo db, dodaj go
            //     if (existingKey == null)
            //     {
            //         //dodaj gi onie shto se pogolemi od momentalnoto vreme
            //         DateTimeKey dateTimeKey = new DateTimeKey
            //         {
            //             Date = formattedDate,
            //             Time = time
            //         };
            //         this._dateTimeKeyRepository.Insert(dateTimeKey);
            //     }
            // }
            //ako ne e prazna db
            // else if (_dateTimeKeyRepository.GetAll().ToList().Count < 16)
            // {
            //     foreach (var time in times)
            //     {
            //         if (currentTime.CompareTo(time) < 0) //vidi dali momentalnoto vreme < times []
            //         {
            //             var existingKey = _dateTimeKeyRepository.GetAll()
            //                 .FirstOrDefault(z => z.Date.Equals(formattedDate) && z.Time.Equals(time));
            //             //ako ne postoi dateTimeKey vo db, dodaj go
            //             if (existingKey == null)
            //             {
            //                 DateTimeKey dateTimeKey = new DateTimeKey
            //                 {
            //                     Date = formattedDate,
            //                     Time = time
            //                 };
            //                 this._dateTimeKeyRepository.Insert(dateTimeKey);
            //             }
            //         }
            //     }
        }


        public void AddSeatMaps(DateTime[] targetDates, string[] times)
        {
            //mesto tret argument list<movie> da gi izvlecham od baza
            var allMovies = this._movieRepository.GetAll().ToList();
            foreach (var targetDate in targetDates)
            {
                string formattedDate = targetDate.ToString("yyyy-MM-dd");
                for (int i = 0; i < times.Length; i++)
                {
                    var time = times[i];
                    var dateTimeKey = this._dateTimeKeyRepository.GetAll()
                        .FirstOrDefault(key => key.Date.Equals(formattedDate) && key.Time.Equals(time));

                    if (dateTimeKey != null)
                    {
                        //checking for seatMaps with the same date and time
                        //ako ne postoi takov seatMap, go kreirame
                        foreach (var movie in allMovies)
                        {
                            var movieId = movie.Id;

                            if (
                                !this._seatMapRepository.GetAll()
                                    .Any(sm => sm.Movie.Id.Equals(movieId) && sm.DateTimeKey.Id.Equals(dateTimeKey.Id))
                            )
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
                                this._seatMapRepository.Insert(seatMap);
                            }
                        }
                    }
                }
            }
        }
    }
}