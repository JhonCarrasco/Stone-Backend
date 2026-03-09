using Stone.Entities;

namespace Stone.Repositories.Interface
{
    public interface IBankAccountRepository : IRepositoryBase<BankAccount>
    {
        Task<BankAccount?> GetAsync(string accountNumber, int bankId, int typeAccount);
    }
}
