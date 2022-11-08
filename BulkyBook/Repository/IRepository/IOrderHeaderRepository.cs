using BulkyBook.Areas.Admin.Models;
using BulkyBook.Areas.Customer.Models;

namespace BulkyBook.Repository.IRepository
{
    public interface IOrderHeaderRepository : IRepository<OrderHeader>
    {
        void update(OrderHeader orderHeader);
        void updateStatus(int id , string orderStatus , string? paymentStatus = null);
        void updateStripePayment(int id, string seeitonId, string paymentItenedId);
    }
}   