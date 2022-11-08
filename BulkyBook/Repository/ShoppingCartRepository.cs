using BulkyBook.Data;
using BulkyBook.Areas.Admin.Models;
using BulkyBook.Repository.IRepository;
using BulkyBook.Areas.Customer.Models;

namespace BulkyBook.Repository
{

    public class ShoppingCartRepository : Repository<ShoppingCart> , IShoppingCartRepository
    {
        private ApplecationDbContext _db;

        public ShoppingCartRepository(ApplecationDbContext db) : base(db)
        {
            _db = db;
        }

        public int DecrementCount(ShoppingCart shoppingCart, int count)
        {
            shoppingCart.count -= count;
            return shoppingCart.count;
        }

        public int IncrementCount(ShoppingCart shoppingCart, int count)
        {
            shoppingCart.count += count;
            return shoppingCart.count;
        }
    }
}