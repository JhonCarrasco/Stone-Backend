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
    public class ExpenseRepository : RepositoryBase<Expense>, IExpenseRepository
    {
        private readonly IHttpContextAccessor httpContextAccessor;

        public ExpenseRepository(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor) : base(context)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        public async Task<ICollection<Expense>?> GetAsync<TKey>(Expression<Func<Expense, bool>> predicate, Expression<Func<Expense, TKey>> orderBy, PaginationDto pagination)
        {
            //eager loading approach optimizado
            var queryable = context.Set<Expense>()
                .Include(x => x.Budget)
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
