using Stone.Dto.Request;
using Stone.Entities;
using System.Linq.Expressions;

namespace Stone.Repositories.Interface
{
    public interface IBudgetRepository : IRepositoryBase<Budget>
    {
        Task<ICollection<Budget>?> GetAsync<TKey>(Expression<Func<Budget, bool>> predicate, Expression<Func<Budget, TKey>> orderBy, PaginationDto pagination);
        Task<int> CountAsync(Expression<Func<Budget, bool>> predicate);
    }
}
