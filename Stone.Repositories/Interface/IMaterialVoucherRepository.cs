using Stone.Dto.Request;
using Stone.Entities;
using System.Linq.Expressions;

namespace Stone.Repositories.Interface
{
    public interface IMaterialVoucherRepository : IRepositoryBase<MaterialVoucher>
    {
        Task<ICollection<MaterialVoucher>?> GetAsync<TKey>(Expression<Func<MaterialVoucher, bool>> predicate, Expression<Func<MaterialVoucher, TKey>> orderBy, PaginationDto pagination);
    }
}
