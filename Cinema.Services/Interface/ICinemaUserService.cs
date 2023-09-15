using System;
using System.Collections.Generic;
using System.Security.Claims;
using Cinema.Models.Identity;

namespace Cinema.Services.Interface
{
    public interface ICinemaUserService
    {
        List<CinemaUser> GetAllCinemaUsers();
        CinemaUser GetDetailsForCinemaUser(string id);
        void CreateNewCinemaUser(CinemaUser cinemaUser);
        void UpdateExistingCinemaUser(CinemaUser cinemaUser);
        void DeleteCinemaUser(string cinemaUserId);
        CinemaUser GetCurrentUser(ClaimsPrincipal claimsPrincipal);
    }
}