using BulkyBook.Data;
using BulkyBook.Repository.IRepository;

namespace BulkyBook.Repository
{
    public class UntiOfWork : IUnitOfWork
    {
        private ApplecationDbContext _db;

        public UntiOfWork(ApplecationDbContext db)
        {
            _db = db;
            categery = new CategeryRepository(_db);
            coverType = new CoverTypeRepository(_db);
            product = new ProductRepository(_db);
            company = new CompanyRepository(_db);
        }

        public ICategeryRepository categery { get; private set; }
        public ICoverTypeRepository coverType { get; private set; }
        public IProductRepository product { get; private set;}
        public ICompanyRepository company { get; private set; }

        public void save()
        {
            _db.SaveChanges();
        }
    }
}