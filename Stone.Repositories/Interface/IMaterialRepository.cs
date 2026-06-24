using Stone.Dto.Request;
using Stone.Entities;
using System.Linq.Expressions;

namespace Stone.Repositories.Interface
{
    public interface IMaterialRepository : IRepositoryBase<Material>
    {
        Task<ICollection<Material>?> GetAsync<TKey>(Expression<Func<Material, bool>> predicate, Expression<Func<Material, TKey>> orderBy, PaginationDto pagination);
        Task<ICollection<Material>?> GetByReceptionIdAsync(int receptionId);
        Task<ICollection<Material>?> GetByDispatchIdAsync(int dispatchId);
        Task<ICollection<Material>?> GetByVoucherIdAsync(int voucherId);
    }
}
