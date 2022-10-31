using BulkyBook.Areas.Admin.Models;

namespace BulkyBook.Repository.IRepository
{
    public interface ICategeryRepository : IRepository<Categery>
    {
        void update(Categery categery);
    }
}