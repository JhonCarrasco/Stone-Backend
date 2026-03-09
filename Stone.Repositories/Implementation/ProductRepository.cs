using Microsoft.EntityFrameworkCore;
using Stone.Entities;
using Stone.Persistence;
using Stone.Repositories.Implements;
using Stone.Repositories.Interface;

namespace Stone.Repositories.Implementation
{
    public class ProductRepository : RepositoryBase<Product>, IProductRepository
    {
        public ProductRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<Product?> GetByCategoyIdAsync(int categoryId)
        {
            return await context.Set<Product>().FirstOrDefaultAsync(x => x.CategoryId == categoryId);
        }

        public async Task<Product?> GetByDescAsync(string description)
        {
            return await context.Set<Product>().FirstOrDefaultAsync(x => x.Description == description);
        }

        public async Task<Product?> GetByManufacturerIdAsync(int manufacturerId)
        {
            return await context.Set<Product>().FirstOrDefaultAsync(x => x.ManufacturerId == manufacturerId);
        }

        public async Task<Product?> GetByProviderIdAsync(int providerId)
        {
            return await context.Set<Product>().FirstOrDefaultAsync(x => x.ProviderId == providerId);
        }
    }
}
