using Microsoft.EntityFrameworkCore;
using Stone.Entities;
using Stone.Persistence;
using Stone.Repositories.Implements;
using Stone.Repositories.Interface;

namespace Stone.Repositories.Implementation
{
    public class LocationRepository : RepositoryBase<Location>, ILocationRepository
    {
        public LocationRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<Location?> GetAsync(string address, int communeId, int regionId)
        {
            return await context.Set<Location>().FirstOrDefaultAsync(x => x.Address == address && x.CommuneId == communeId && x.RegionId == regionId);
        }

    }
}
