using System.Linq.Expressions;
using BulkyBook.Areas.Admin.Data;
using BulkyBook.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

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

        public IEnumerable<T> getAll()
        {
            IQueryable<T> query = dbSet;
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

        public T getFirstOrDefault(Expression<Func<T, bool>> filter)
        {
            IQueryable<T> query = dbSet;
            query = query.Where(filter);
            return query.FirstOrDefault();
        }
    }
}