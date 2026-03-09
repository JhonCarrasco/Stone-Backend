using Microsoft.EntityFrameworkCore;
using Stone.Entities;
using Stone.Persistence;
using Stone.Repositories.Implements;
using Stone.Repositories.Interface;

namespace Stone.Repositories.Implementation
{
    public class ContactRepository : RepositoryBase<Contact>, IContactRepository
    {
        public ContactRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<ICollection<Contact>> GetAsyncbyCustomerId(int customerId)
        {
            return await context.Set<Contact>()
                .Include(x => x.Person)
                .Include(x => x.Provider)
                .Include(x => x.Customer)
                .Where(x => x.CustomerId == customerId)
                .OrderBy(x => x.Person!.DisplayName)
                .AsNoTracking()
                .IgnoreQueryFilters()//traer data aunque relacion haya sido eliminado
                .ToListAsync();
        }
    }
}
