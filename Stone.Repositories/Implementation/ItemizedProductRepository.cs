using Stone.Entities;
using Stone.Persistence;
using Stone.Repositories.Implements;
using Stone.Repositories.Interface;

namespace Stone.Repositories.Implementation
{
    public class ItemizedProductRepository : RepositoryBase<ItemizedProduct>, IItemizedProductRepository
    {
        public ItemizedProductRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
