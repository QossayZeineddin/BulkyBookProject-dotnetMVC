using BulkyBook.Data;
using BulkyBook.Areas.Admin.Models;
using BulkyBook.Repository.IRepository;
using BulkyBook.Areas.Customer.Models;
using BulkyBook.PublicModels;

namespace BulkyBook.Repository
{

    public class ApplcationUserRepository : Repository<ApplecationUser> , IApplcationUserRepository
    {
        private ApplecationDbContext _db;

        public ApplcationUserRepository(ApplecationDbContext db) : base(db)
        {
            _db = db;
        }

       

       
    }
}