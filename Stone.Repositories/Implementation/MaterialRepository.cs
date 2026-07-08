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
    public class MaterialRepository : RepositoryBase<Material>, IMaterialRepository
    {
        private readonly IHttpContextAccessor httpContextAccessor;//forma de acceder a los headers
        public MaterialRepository(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor) : base(context)
        {
            this.httpContextAccessor = httpContextAccessor;
        }           

        public async Task<ICollection<Material>?> GetAsync<TKey>(Expression<Func<Material, bool>> predicate, Expression<Func<Material, TKey>> orderBy, PaginationDto pagination)
        {
            //eager loading approach optimizado
            var queryable = context.Set<Material>()
                //.Include(x => x.Product)
                .Where(predicate)
                .OrderBy(orderBy)
                .AsNoTracking()
                .IgnoreQueryFilters()//traer data aunque relacion haya sido eliminado                
                .AsQueryable();

            await httpContextAccessor.HttpContext.InsertarPaginacionHeader(queryable);
            var response = await queryable.Paginate(pagination).ToListAsync();
            return response;
        }

        public async Task<ICollection<Material>?> GetByDispatchIdAsync(int dispatchId)
        {
            var queryable = context.Set<Material>()
                .Where(m => m.DispatchId == dispatchId && m.Active)
                .AsNoTracking()
                .IgnoreQueryFilters()
                .AsQueryable();

            var response = await queryable.ToListAsync();
            return response;
        }

        public async Task<ICollection<Material>?> GetByReceptionIdAsync(int receptionId)
        {
            var queryable = context.Set<Material>()
                .Where(m => m.ReceptionId == receptionId && m.Active)
                .AsNoTracking()
                .IgnoreQueryFilters()
                .AsQueryable();

            var response = await queryable.ToListAsync();
            return response;
        }

        public async Task<ICollection<Material>?> GetByVoucherIdAsync(int voucherId)
        {
            var queryable = context.Set<Material>()
                .Where(m => m.VoucherId == voucherId && m.Active)
                .AsNoTracking()
                .IgnoreQueryFilters()
                .AsQueryable();

            var response = await queryable.ToListAsync();
            return response;
        }
        
    }
}
