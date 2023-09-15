using System;
using System.Collections.Generic;
using System.Linq;
using Cinema.Models.Domain;
using Cinema.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace Cinema.Repository.Implementation
{
    public class Repository<T> : IRepository<T> where T : BaseEntity
    {
        private readonly ApplicationDbContext _context;
        private DbSet<T> entities;
        string errorMessage = string.Empty;
        
        public Repository(ApplicationDbContext context)
        {
            _context = context;
            entities = context.Set<T>();
        }

        public IEnumerable<T> GetAll()
        {
            return this.entities.AsEnumerable();
        }

        public T Get(Guid? id)
        {
            return this.entities.SingleOrDefault(z => z.Id.Equals(id));
        }

        public void Insert(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }

            this.entities.Add(entity);
            _context.SaveChanges();
        }

        public void Update(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            entities.Update(entity);
            _context.SaveChanges();
        }

        public void Delete(T entity)
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