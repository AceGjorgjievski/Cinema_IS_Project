using System;

namespace Cinema.Services.Interface
{
    public interface IHomeService
    {
        void DeleteDateTimeKeysAndRelatedSeatMaps(DateTime now, string currentTime);
        void addDateTimeKeys(DateTime[] targetDates, string[] times);
        void AddSeatMaps(DateTime [] targetDates, string [] times);
    }
}