using BulkyBook.Areas.Admin.Data;
using BulkyBook.Areas.Admin.Models;
using BulkyBook.Repository.IRepository;

namespace BulkyBook.Repository
{

    public class CategeryRepository : Repository<Categery> , ICategeryRepository
    {
        private ApplecationDbContext _db;

        public CategeryRepository(ApplecationDbContext db) : base(db)
        {
            _db = db;
        }

        public void update(Categery categery)
        {
            _db.categeries.Update(categery);
        }

       
    }
}