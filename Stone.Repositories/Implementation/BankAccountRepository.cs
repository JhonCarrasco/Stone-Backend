using Microsoft.EntityFrameworkCore;
using Stone.Entities;
using Stone.Persistence;
using Stone.Repositories.Implements;
using Stone.Repositories.Interface;

namespace Stone.Repositories.Implementation
{
    public class BankAccountRepository : RepositoryBase<BankAccount>, IBankAccountRepository
    {
        public BankAccountRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<BankAccount?> GetAsync(string accountNumber, int bankId, int typeAccount)
        {
            return await context.Set<BankAccount>().FirstOrDefaultAsync(x => x.Account == accountNumber && x.BankId == bankId && x.TypeAccountId == typeAccount);
        }
    }
}
