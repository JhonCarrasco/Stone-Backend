using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Stone.Dto.Request;
using Stone.Entities;
using Stone.Persistence;
using Stone.Repositories.Implements;
using Stone.Repositories.Interface;
using Stone.Repositories.Utils;
using System.Linq.Expressions;

namespace Stone.Repositories.Implementation
{
    public class ProductRepository : RepositoryBase<Product>, IProductRepository
    {
        private readonly IHttpContextAccessor httpContextAccessor;//forma de acceder a los headers
        public ProductRepository(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor) : base(context)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        public async Task<int> CountAsync(Expression<Func<Product, bool>> predicate)
        {
            return await context.Set<Product>()
                .Where(predicate)
                .AsNoTracking()
                .IgnoreQueryFilters()//traer data aunque relacion haya sido eliminado 
                .CountAsync();
        }

        public async Task<ICollection<Product>> GetAsync<TKey>(Expression<Func<Product, bool>> predicate, Expression<Func<Product, TKey>> orderBy, PaginationDto pagination)
        {
            ////eager loading approach optimizado
            var queryable = context.Set<Product>()
                //.Include(x => x.Manufacturer)
                //.Include(x => x.Category)
                //.Include(x => x.Provider)
                //.ThenInclude(x => x.Genre) // Concert --> Genre
                .Where(predicate)
                .OrderBy(orderBy)
                .AsNoTracking()
                .IgnoreQueryFilters()//traer data aunque relacion haya sido eliminado                
                .AsQueryable();

            await httpContextAccessor.HttpContext.InsertarPaginacionHeader(queryable);
            var response = await queryable.Paginate(pagination).ToListAsync();
            return response;
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
