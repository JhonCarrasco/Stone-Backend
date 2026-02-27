using Stone.Dto.Request;
using Stone.Entities;
using Stone.Entities.Info;

namespace Stone.Repositories.Interface
{
    public interface IConcertRepository : IRepositoryBase<Concert>
    {
        Task<ICollection<ConcertInfo>> GetAsync(string? title, PaginationDto pagination);
        Task FinalizeAsync(int id);
    }
}
