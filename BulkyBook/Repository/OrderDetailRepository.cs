using BulkyBook.Data;
using BulkyBook.Areas.Admin.Models;
using BulkyBook.Repository.IRepository;
using BulkyBook.Areas.Customer.Models;

namespace BulkyBook.Repository
{

    public class OrderDetailRepository : Repository<OrderDetail> , IOrderDetailRepository
    {
        private ApplecationDbContext _db;

        public OrderDetailRepository(ApplecationDbContext db) : base(db)
        {
            _db = db;
        }

        public void update(OrderDetail orderDetail)
        {
            _db.orderDetails.Update(orderDetail);
        }

       
    }
}