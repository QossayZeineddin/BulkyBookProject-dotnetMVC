using BulkyBook.Areas.Admin.Models;

namespace BulkyBook.Repository.IRepository
{
    public interface ICompanyRepository :IRepository<Company>
    {
        void update(Company company);

    }
}
