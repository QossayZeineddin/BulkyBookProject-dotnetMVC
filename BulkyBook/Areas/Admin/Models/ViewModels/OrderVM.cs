using BulkyBook.Areas.Customer.Models;

namespace BulkyBook.Areas.Admin.Models.ViewModels
{
    public class OrderVM
    {
        public OrderHeader orderHeader { get; set; }
        public IEnumerable<OrderDetail> orderDetails { get; set; }
    }
} 