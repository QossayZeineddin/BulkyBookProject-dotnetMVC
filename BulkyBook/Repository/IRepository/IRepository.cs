using System.Linq.Expressions;

namespace BulkyBook.Repository.IRepository
{

    public interface IRepository<T> where T : class
    {
        IEnumerable<T> getAll(string? includeProperies = null);
        IEnumerable<T> getAllByUserId(Expression<Func<T, bool>> filter, string? includeProperies = null );


        void add(T entitiy);

        void remove(T entitiy);

        void removeRange(IEnumerable<T> entitiy);


        T getFirstOrDefault(Expression<Func<T, bool>> filter, string? includeProperies = null ,  bool tracked = true);
    }
}