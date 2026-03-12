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
    public class BudgetRepository : RepositoryBase<Budget>, IBudgetRepository
    {
        private readonly IHttpContextAccessor httpContextAccessor;

        public BudgetRepository(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor) : base(context)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        public async Task<int> CountAsync(Expression<Func<Budget, bool>> predicate)
        {
            return await context.Set<Budget>()
                .Where(predicate)
                .AsNoTracking()
                .IgnoreQueryFilters()//traer data aunque relacion haya sido eliminado 
                .CountAsync();
        }

        public async Task<ICollection<Budget>?> GetAsync<TKey>(Expression<Func<Budget, bool>> predicate, Expression<Func<Budget, TKey>> orderBy, PaginationDto pagination)
        {
            ////eager loading approach optimizado
            var queryable = context.Set<Budget>()
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
    }
}
