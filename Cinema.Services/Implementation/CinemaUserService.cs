using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Cinema.Models.Identity;
using Cinema.Repository.Interface;
using Cinema.Services.Interface;

namespace Cinema.Services.Implementation
{
    public class CinemaUserService : ICinemaUserService
    {
        private readonly ICinemaUserRepository _cinemaUserRepository;
        
        public CinemaUserService(ICinemaUserRepository cinemaUserRepository)
        {
            _cinemaUserRepository = cinemaUserRepository;
        }

        public List<CinemaUser> GetAllCinemaUsers()
        {
            return this._cinemaUserRepository.GetAll().ToList();
        }

        public CinemaUser GetDetailsForCinemaUser(string id)
        {
            return this._cinemaUserRepository.Get(id);
        }

        public void CreateNewCinemaUser(CinemaUser cinemaUser)
        {
            this._cinemaUserRepository.Insert(cinemaUser);
        }

        public void UpdateExistingCinemaUser(CinemaUser cinemaUser)
        {
            this._cinemaUserRepository.Update(cinemaUser);
        }

        public void DeleteCinemaUser(string cinemaUserId)
        {
            CinemaUser cinemaUser = this.GetDetailsForCinemaUser(cinemaUserId);
            this._cinemaUserRepository.Delete(cinemaUser);
        }

        public CinemaUser GetCurrentUser(ClaimsPrincipal claimsPrincipal)
        {
            var customerIdClaim = claimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier);
            //userId
            if (customerIdClaim != null)
            {
                var cinemaUser = _cinemaUserRepository.Get(customerIdClaim);

                return cinemaUser;
            }

            return null;
        }
    }
}