namespace BulkyBook.Repository.IRepository
{
    public interface IUnitOfWork
    {
        ICategeryRepository categery { get; }
        ICoverTypeRepository coverType { get; }
        IProductRepository product { get; }
        ICompanyRepository company { get; }

        void save();
    }
}