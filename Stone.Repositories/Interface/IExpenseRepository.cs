using Stone.Dto.Request;
using Stone.Entities;
using System.Linq.Expressions;

namespace Stone.Repositories.Interface
{
    public interface IExpenseRepository : IRepositoryBase<Expense>
    {
        Task<ICollection<Expense>?> GetAsync<TKey>(Expression<Func<Expense, bool>> predicate, Expression<Func<Expense, TKey>> orderBy, PaginationDto pagination);
    }
}
