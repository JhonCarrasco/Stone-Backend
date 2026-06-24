using Stone.Dto.Request;
using Stone.Entities;
using System.Linq.Expressions;

namespace Stone.Repositories.Interface
{
    public interface IDispatchGuideRepository : IRepositoryBase<DispatchGuide>
    {
        Task<ICollection<DispatchGuide>?> GetAsync<TKey>(Expression<Func<DispatchGuide, bool>> predicate, Expression<Func<DispatchGuide, TKey>> orderBy, PaginationDto pagination);
    }
}
