using System;
using System.Collections.Generic;
using Cinema.Models.Identity;

namespace Cinema.Repository.Interface
{
    public interface ICinemaUserRepository
    {
        IEnumerable<CinemaUser> GetAll();
        CinemaUser Get(string id);
        void Insert(CinemaUser entity);
        void Update(CinemaUser entity);
        void Delete(CinemaUser entity);
    }
}