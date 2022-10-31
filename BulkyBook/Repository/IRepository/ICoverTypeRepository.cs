using BulkyBook.Areas.Admin.Models;

namespace BulkyBook.Repository.IRepository
{
    public interface ICoverTypeRepository : IRepository<CoverType>
    {
        void update(CoverType coverType);
    }
}