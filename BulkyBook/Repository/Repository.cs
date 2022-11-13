using BulkyBook.Data;
using BulkyBook.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BulkyBook.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        public readonly ApplecationDbContext _db;
        internal DbSet<T> dbSet;

        public Repository(ApplecationDbContext db)
        {
            _db = db;
            this.dbSet = _db.Set<T>();
        }

        public IEnumerable<T> getAll(string? includeProperies = null)
        {
            IQueryable<T> query = dbSet;
            if (includeProperies != null)
            {
                foreach (var includeProp in includeProperies.Split(new char[] { ',' },
                             StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp);
                }
            }

            return query.ToList();
        }

        public IEnumerable<T> getAllByUserId(Expression<Func<T, bool>> filter, string? includeProperies = null)
        {
            IQueryable<T> query = dbSet;
            query = query.Where(filter);

            if (includeProperies != null)
            {
                foreach (var includeProp in includeProperies.Split(new char[] { ',' },
                             StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp);
                }
            }

            return query.ToList();
        }

        public void add(T entitiy)
        {
            dbSet.Add(entitiy);
        }

        public void remove(T entitiy)
        {
            dbSet.Remove(entitiy);
        }

        public void removeRange(IEnumerable<T> entitiy)
        {
            dbSet.RemoveRange(entitiy);
        }

        public T getFirstOrDefault(Expression<Func<T, bool>> filter, string? includeProperies = null,
            bool tracked = true)
        {
            IQueryable<T> query;
            if (tracked == true)
            {
                query = dbSet;
            }
            else
            {
                query = dbSet.AsNoTracking();
            }

            query = query.Where(filter);
            
            if (includeProperies != null)
            {
                foreach (var includeProp in includeProperies.Split(new char[] { ',' },
                             StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp);
                }
            }

            return query.FirstOrDefault();
        }
    }
}