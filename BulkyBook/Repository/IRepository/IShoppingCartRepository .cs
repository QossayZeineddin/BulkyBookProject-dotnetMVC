using BulkyBook.Areas.Admin.Models;
using BulkyBook.Areas.Customer.Models;

namespace BulkyBook.Repository.IRepository
{
    public interface IShoppingCartRepository : IRepository<ShoppingCart>
    {
        int IncrementCount(ShoppingCart shoppingCart , int count);
        int DecrementCount(ShoppingCart shoppingCart, int count);
    }
}