using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Stone.Dto.Request;
using Stone.Entities;
using Stone.Persistence;
using Stone.Repositories.Implements;
using Stone.Repositories.Interface;
using Stone.Repositories.Utils;
using System.Linq;
using System.Linq.Expressions;

namespace Stone.Repositories.Implementation
{
    public class CustomerRepository : RepositoryBase<Customer>, ICustomerRepository
    {
        private readonly IHttpContextAccessor httpContextAccessor;

        public CustomerRepository(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor) : base(context)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        public async Task<int> CountAsync(Expression<Func<Customer, bool>> predicate)
        {
            return await context.Set<Customer>()
                .Where(predicate)
                .AsNoTracking()
                .IgnoreQueryFilters()//traer data aunque relacion haya sido eliminado 
                .CountAsync();
        }

        public async Task<ICollection<Customer>?> GetAsync<TKey>(Expression<Func<Customer, bool>> predicate, Expression<Func<Customer, TKey>> orderBy, PaginationDto pagination)
        {
            ////eager loading approach optimizado
            var queryable = context.Set<Customer>()
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

        public async Task<Customer?> GetByEmailAsync(string email)
        {
            return await context.Set<Customer>().FirstOrDefaultAsync(x => x.Email == email);
        }

    }
}
