using Stone.Entities;

namespace Stone.Repositories.Interface
{
    public interface ICustomerRepository : IRepositoryBase<Customer>
    {
        Task<Customer?> GetByEmailAsync(string email);
    }
}
