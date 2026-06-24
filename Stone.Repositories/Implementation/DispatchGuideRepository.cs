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
    public class DispatchGuideRepository : RepositoryBase<DispatchGuide>, IDispatchGuideRepository
    {
        private readonly IHttpContextAccessor httpContextAccessor;//forma de acceder a los headers
        public DispatchGuideRepository(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor) : base(context)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        public async Task<ICollection<DispatchGuide>?> GetAsync<TKey>(Expression<Func<DispatchGuide, bool>> predicate, Expression<Func<DispatchGuide, TKey>> orderBy, PaginationDto pagination)
        {
            //eager loading approach optimizado
            var queryable = context.Set<DispatchGuide>()
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
