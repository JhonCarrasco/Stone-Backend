using Stone.Entities;

namespace Stone.Repositories.Interface
{
    public interface IProductRepository: IRepositoryBase<Product>
    {
        Task<Product?> GetByDescAsync(string description);
        Task<Product?> GetByProviderIdAsync(int providerId);
        Task<Product?> GetByCategoyIdAsync(int categoryId);
        Task<Product?> GetByManufacturerIdAsync(int manufacturerId);
    }
}
