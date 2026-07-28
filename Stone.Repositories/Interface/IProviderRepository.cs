using Stone.Dto.Request;
using Stone.Entities;
using System.Linq.Expressions;

namespace Stone.Repositories.Interface
{
    public interface IProviderRepository : IRepositoryBase<Provider>
    {
        Task<ICollection<Provider>?> GetAsync<TKey>(Expression<Func<Provider, bool>> predicate, Expression<Func<Provider, TKey>> orderBy, PaginationDto pagination);
    }
}
