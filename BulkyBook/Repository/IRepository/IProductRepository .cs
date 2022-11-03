using BulkyBook.Areas.Admin.Models;

namespace BulkyBook.Repository.IRepository
{
    public interface IProductRepository : IRepository<Product>
    {
        void update(Product product);
    }
}   