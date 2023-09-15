using System;
using System.Collections.Generic;
using System.Linq;
using Cinema.Models.Domain;
using Cinema.Repository.Interface;
using Cinema.Services.Interface;

namespace Cinema.Services.Implementation
{
    public class DateTimeKeyService : IDateTimeKeyService
    {
        private readonly IRepository<DateTimeKey> _dateTimeKeyRepository;
        
        public DateTimeKeyService(IRepository<DateTimeKey> dateTimeKeyRepository)
        {
            _dateTimeKeyRepository = dateTimeKeyRepository;
        }
        

        public List<DateTimeKey> GetAllDateTimeKeys()
        {
            return this._dateTimeKeyRepository.GetAll().ToList();
        }

        public DateTimeKey GetDetailsForDateTimeKey(Guid? id)
        {
            return this._dateTimeKeyRepository.Get(id);
        }

        public void CreateNewDateTimeKey(DateTimeKey dateTimeKey)
        {
            this._dateTimeKeyRepository.Insert(dateTimeKey);
        }

        public void UpdateExistingDateTimeKey(DateTimeKey dateTimeKey)
        {
            this._dateTimeKeyRepository.Update(dateTimeKey);
        }

        public void DeleteDateTimeKey(Guid id)
        {
            DateTimeKey dateTimeKey = this.GetDetailsForDateTimeKey(id);
            this._dateTimeKeyRepository.Delete(dateTimeKey);
        }
    }
}