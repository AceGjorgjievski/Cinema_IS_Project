using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Cinema.Models.Domain;

namespace Cinema.Repository.Interface
{
    public interface IRepository<T> where T : BaseEntity
    {
        IEnumerable<T> GetAll();
        T Get(Guid? id);
        void Insert(T entity);
        void Update(T entity);
        void Delete(T entity);
        
        IQueryable<T> GetAllIncluding(params Expression<Func<T, object>>[] includeProperties);
    }
}