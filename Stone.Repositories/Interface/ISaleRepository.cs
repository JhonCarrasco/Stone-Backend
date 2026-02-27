using Stone.Entities.Info;
using Stone.Entities;
using System.Linq.Expressions;
using Stone.Dto.Request;

namespace Stone.Repositories.Interface
{
    public interface ISaleRepository : IRepositoryBase<Sale>
    {
        Task CreateTransactionAsync();
        Task RollBackAsync();
        Task<ICollection<Sale>> GetAsync<TKey>(Expression<Func<Sale, bool>> predicate, Expression<Func<Sale, TKey>> orderBy,
            PaginationDto pagination);
        Task<ICollection<ReportInfo>> GetSaleReportAsync(DateTime dateStart, DateTime dateEnd);
    }
}
