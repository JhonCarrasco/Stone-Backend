using Stone.Entities;
using Stone.Persistence;
using Stone.Repositories.Implements;
using Stone.Repositories.Interface;

namespace Stone.Repositories.Implementation
{
    public class BankRepository : RepositoryBase<Bank>, IBankRepository
    {
        public BankRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
