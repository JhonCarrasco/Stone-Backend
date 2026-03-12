using Stone.Entities;
using Stone.Persistence;
using Stone.Repositories.Implements;
using Stone.Repositories.Interface;

namespace Stone.Repositories.Implementation
{
    public class ItemizedServiceRepository : RepositoryBase<ItemizedService>, IItemizedServiceRepository
    {
        public ItemizedServiceRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
