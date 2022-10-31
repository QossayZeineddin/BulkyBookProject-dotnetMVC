using System.Linq.Expressions;

namespace BulkyBook.Repository.IRepository
{

    public interface IRepository<T> where T : class
    {
        IEnumerable<T> getAll();

        void add(T entitiy);

        void remove(T entitiy);

        void removeRange(IEnumerable<T> entitiy);


        T getFirstOrDefault(Expression<Func<T, bool>> filter);
    }
}