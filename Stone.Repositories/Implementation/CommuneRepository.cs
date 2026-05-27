using Stone.Entities;
using Stone.Persistence;
using Stone.Repositories.Implements;
using Stone.Repositories.Interface;

namespace Stone.Repositories.Implementation
{
    public class CommuneRepository : RepositoryBase<Commune>, ICommuneRepository
    {
        public CommuneRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
