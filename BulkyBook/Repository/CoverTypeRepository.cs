using BulkyBook.Data;
using BulkyBook.Areas.Admin.Models;
using BulkyBook.Repository.IRepository;

namespace BulkyBook.Repository
{

    public class CoverTypeRepository : Repository<CoverType> , ICoverTypeRepository
    {
        private ApplecationDbContext _db;

        public CoverTypeRepository(ApplecationDbContext db) : base(db)
        {
            _db = db;
        }

        public void update(CoverType obj)
        {
            _db.CoverTypes.Update(obj);
        }

       
    }
}