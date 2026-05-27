using Stone.Dto.Request;
using Stone.Entities;
using System.Linq.Expressions;

namespace Stone.Repositories.Interface
{
    public interface ICustomerRepository : IRepositoryBase<Customer>
    {
        Task<Customer?> GetByEmailAsync(string email);
        Task<int> CountAsync(Expression<Func<Customer, bool>> predicate);
        Task<ICollection<Customer>?> GetAsync<TKey>(Expression<Func<Customer, bool>> predicate, Expression<Func<Customer, TKey>> orderBy, PaginationDto pagination);
    }
}
