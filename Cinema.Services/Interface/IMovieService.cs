using System;
using System.Collections.Generic;
using Cinema.Models.Domain;

namespace Cinema.Services.Interface
{
    public interface IMovieService
    {
        List<Movie> GetAllMovies();
        Movie GetDetailsForMovie(Guid? id);
        void CreateNewMovie(Movie m);
        void UpdateExistingMovie(Movie m);
        void DeleteMovie(Guid id);
        MovieBookDto GetMovieBookDto(Movie movie2, string time, string date);
        void MakeSeatsAvailableAgain(Guid? movieId, string time, string date);
    }
}