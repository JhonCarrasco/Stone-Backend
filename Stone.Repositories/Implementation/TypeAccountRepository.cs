using Stone.Entities;
using Stone.Persistence;
using Stone.Repositories.Implements;
using Stone.Repositories.Interface;

namespace Stone.Repositories.Implementation
{
    public class TypeAccountRepository : RepositoryBase<TypeAccount>, ITypeAccountRepository
    {
        public TypeAccountRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
