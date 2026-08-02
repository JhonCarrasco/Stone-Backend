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
    public class ProviderRepository : RepositoryBase<Provider>, IProviderRepository
    {
        private readonly IHttpContextAccessor httpContextAccessor;//forma de acceder a los headers
        public ProviderRepository(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor) : base(context)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        public async Task<ICollection<Provider>?> GetAsync<TKey>(Expression<Func<Provider, bool>> predicate, Expression<Func<Provider, TKey>> orderBy, PaginationDto pagination)
        {
            //eager loading approach optimizado
            var queryable = context.Set<Provider>()
                .Include(x => x.Person)
                .Include(x => x.Location)
                .Include(x => x.BankAccount)
                .Include(x => x.Contacts)
                .Where(predicate)
                .OrderBy(orderBy)
                .AsNoTracking()
                .IgnoreQueryFilters()//traer data aunque relacion haya sido eliminado                
                .AsQueryable();

            await httpContextAccessor.HttpContext.InsertarPaginacionHeader(queryable);
            var response = await queryable.Paginate(pagination).ToListAsync();
            return response;
        }
    }
}
