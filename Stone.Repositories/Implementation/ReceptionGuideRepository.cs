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
    public class ReceptionGuideRepository : RepositoryBase<ReceptionGuide>, IReceptionGuideRepository
    {
        private readonly IHttpContextAccessor httpContextAccessor;

        public ReceptionGuideRepository(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor) : base(context)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        public async Task<ICollection<ReceptionGuide>?> GetAsync<TKey>(Expression<Func<ReceptionGuide, bool>> predicate, Expression<Func<ReceptionGuide, TKey>> orderBy, PaginationDto pagination)
        {
            //eager loading approach optimizado
            var queryable = context.Set<ReceptionGuide>()
                .Include(x => x.Customer)
                .Include(x => x.Provider)
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
