using System;
using System.Collections.Generic;
using Cinema.Models.Domain;

namespace Cinema.Services.Interface
{
    public interface IDateTimeKeyService
    {
        List<DateTimeKey> GetAllDateTimeKeys();
        DateTimeKey GetDetailsForDateTimeKey(Guid? id);
        void CreateNewDateTimeKey(DateTimeKey dateTimeKey);
        void UpdateExistingDateTimeKey(DateTimeKey dateTimeKey);
        void DeleteDateTimeKey(Guid id);
    }
}