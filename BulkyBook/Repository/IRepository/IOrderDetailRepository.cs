using BulkyBook.Areas.Admin.Models;
using BulkyBook.Areas.Customer.Models;

namespace BulkyBook.Repository.IRepository
{
    public interface IOrderDetailRepository : IRepository<OrderDetail>
    {
        void update(OrderDetail orderDetail);
    }
}   