using System;
using System.Collections.Generic;
using System.Linq;
using Cinema.Models.Identity;
using Cinema.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace Cinema.Repository.Implementation
{
    public class CinemaUserRepository : ICinemaUserRepository
    {
        private readonly ApplicationDbContext _context;
        private DbSet<CinemaUser> entities;
        string errorMessage = string.Empty;
        
        public CinemaUserRepository(ApplicationDbContext context)
        {
            _context = context;
            entities = context.Set<CinemaUser>();
        }

        public IEnumerable<CinemaUser> GetAll()
        {
            return this.entities.AsEnumerable();
        }

        public CinemaUser Get(string id)
        {
            return this.entities
                .Include(z => z.Orders)
                .SingleOrDefault(z => z.Id.Equals(id));
        }

        public void Insert(CinemaUser entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }

            this.entities.Add(entity);
            _context.SaveChanges();
        }

        public void Update(CinemaUser entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            entities.Update(entity);
            _context.SaveChanges();
        }

        public void Delete(CinemaUser entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            entities.Remove(entity);
            _context.SaveChanges();
        }
    }
}