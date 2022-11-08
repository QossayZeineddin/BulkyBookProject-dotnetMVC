namespace BulkyBook.Repository.IRepository
{
    public interface IUnitOfWork
    {
        ICategeryRepository categery { get; }
        ICoverTypeRepository coverType { get; }
        IProductRepository product { get; }
        ICompanyRepository company { get; }
        IShoppingCartRepository shoppingCart { get; }
        IApplcationUserRepository applcationUser { get; }
        IOrderDetailRepository orderDetail { get; }
        IOrderHeaderRepository orderHeader { get; }

        void save();
    }
}