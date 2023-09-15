using System;

namespace Cinema.Models.Domain
{
    public class DateTimeKey : BaseEntity
    {
        // public Guid Id { get; set; }
        public string? Date { get; set; }
        public string? Time { get; set; }

        public DateTimeKey()
        {
        }

        public DateTimeKey(string date, string time)
        {
            Date = date;
            Time = time;
        }

        public override bool Equals(object obj)
        {
            if (obj is DateTimeKey other)
            {
                return Date == other.Date && Time == other.Time;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Date, Time);
        }
    }

}