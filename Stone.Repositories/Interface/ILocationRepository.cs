using Stone.Entities;

namespace Stone.Repositories.Interface
{
    public interface ILocationRepository : IRepositoryBase<Location>
    {
        Task<Location?> GetAsync(string address, int communeId, int regionId);
    }
}
