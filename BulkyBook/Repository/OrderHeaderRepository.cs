using BulkyBook.Data;
using BulkyBook.Areas.Admin.Models;
using BulkyBook.Repository.IRepository;
using BulkyBook.Areas.Customer.Models;

namespace BulkyBook.Repository
{

    public class OrderHeaderRepository : Repository<OrderHeader> , IOrderHeaderRepository
    {
        private ApplecationDbContext _db;

        public OrderHeaderRepository(ApplecationDbContext db) : base(db)
        {
            _db = db;
        }

        public void update(OrderHeader orderHeader)
        {
            _db.orderHeaders.Update(orderHeader);
        }

        public void updateStatus(int id, string orderStatus, string? paymentStatus = null)
        {
            var orderFromDb = _db.orderHeaders.FirstOrDefault(u => u.id == id);
            if (orderFromDb != null)
            {
                orderFromDb.orderStatus = orderStatus;
                if(paymentStatus != null)
                {
                    orderFromDb.paymentStatus = paymentStatus;
                }
            }
        }

        public void updateStripePayment(int id, string seeitonId, string paymentItenedId)
        {
            var orderFromDb = _db.orderHeaders.FirstOrDefault(u => u.id == id);
            if (orderFromDb != null)
            {
                orderFromDb.SessionId = seeitonId;
                orderFromDb.PaymentIntentId = paymentItenedId;
                
            }
        }
    }
}