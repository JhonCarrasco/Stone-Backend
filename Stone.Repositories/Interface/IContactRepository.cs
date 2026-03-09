using Stone.Entities;

namespace Stone.Repositories.Interface
{
    public interface IContactRepository : IRepositoryBase<Contact>
    {
        Task<ICollection<Contact>> GetAsyncbyCustomerId(int customerId);
    }    
}
