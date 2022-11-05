using BulkyBook.Areas.Admin.Models;
using BulkyBook.Data;
using BulkyBook.Repository.IRepository;

namespace BulkyBook.Repository
{
    public class CompanyRepository : Repository<Company> , ICompanyRepository
    {
        private ApplecationDbContext _db;
        public CompanyRepository(ApplecationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Company company)
        {
            _db.companies.Update(company);
        }

        public void update(Company company)
        {
            _db.companies.Update(company);
        }
    }
}
