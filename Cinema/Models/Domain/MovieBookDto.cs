using System;
using System.Collections.Generic;
using System.Linq;

namespace Cinema.Models.Domain
{
    public class MovieBookDto
    {
        public Guid? Id { get; set; }
        public DateTime DateNow { get; set; }
        public DateTime MinDate { get; set; }
        public DateTime MaxDate { get; set; }
        public string TimeNow { get; set; }
        public string MovieName { get; set; }
        public string MovieImage { get; set; }
        
        public string? SelectedTime { get; set; }
        public string? SelectedDate { get; set; }
        public Movie Movie { get; set; }
        public string [] availableTimes = {"09:00", "12:00", "15:00", "18:00"};
        
        public SeatMap SeatMap { get; set; }
        public DateTimeKey DateTimeKey { get; set; } = new DateTimeKey();
        
        

        public MovieBookDto(Movie movie, string time, string date)
        {
            this.DateTimeKey.Date = date != null ? DateTime.Parse(date).ToString("yyyy-MM-dd") : DateTime.Now.ToString("yyyy-MM-dd");
            this.DateTimeKey.Time = time ?? this.GetDefaultTime();
            this.DateNow = DateTime.Now;
            this.MinDate = DateTime.Now;
            this.TimeNow = DateTime.Now.ToShortTimeString();
            this.MaxDate = DateTime.Now.AddDays(3);
            this.Movie = movie;
            if (this.Movie != null)
            {
                this.MovieName = movie.Name;
                this.MovieImage = movie.Image;
            }
        }

        private string GetDefaultTime()
        {
            TimeSpan currentTime = DateTime.Now.TimeOfDay;

            // Find the next available time based on the current time
            foreach (var availableTime in this.availableTimes)
            {
                TimeSpan parsedTime = TimeSpan.Parse(availableTime);
                if (parsedTime > currentTime)
                {
                    return availableTime;
                }
            }

            // If no available time is found, default to the last time (18:00)
            return this.availableTimes[this.availableTimes.Length - 1];
        }
    }
}