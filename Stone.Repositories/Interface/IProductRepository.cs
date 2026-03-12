using Stone.Dto.Request;
using Stone.Entities;
using System.Linq.Expressions;

namespace Stone.Repositories.Interface
{
    public interface IProductRepository: IRepositoryBase<Product>
    {
        Task<ICollection<Product>?> GetAsync<TKey>(Expression<Func<Product, bool>> predicate, Expression<Func<Product, TKey>> orderBy, PaginationDto pagination);
        Task<Product?> GetByDescAsync(string description);
        Task<Product?> GetByProviderIdAsync(int providerId);
        Task<Product?> GetByCategoyIdAsync(int categoryId);
        Task<Product?> GetByManufacturerIdAsync(int manufacturerId);
        Task<int> CountAsync(Expression<Func<Product, bool>> predicate);
    }
}