using Stone.Dto.Request;
using Stone.Entities;
using System.Linq.Expressions;

namespace Stone.Repositories.Interface
{
    public interface IReceptionGuideRepository : IRepositoryBase<ReceptionGuide>
    {
        Task<ICollection<ReceptionGuide>?> GetAsync<TKey>(Expression<Func<ReceptionGuide, bool>> predicate, Expression<Func<ReceptionGuide, TKey>> orderBy, PaginationDto pagination);
    }
}
