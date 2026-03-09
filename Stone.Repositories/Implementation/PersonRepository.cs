using Microsoft.EntityFrameworkCore;
using Stone.Entities;
using Stone.Persistence;
using Stone.Repositories.Implements;
using Stone.Repositories.Interface;

namespace Stone.Repositories.Implementation
{
    public class PersonRepository : RepositoryBase<Person>, IPersonRepository
    {
        public PersonRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<Person?> GetByRutAsync(string Rut)
        {
            return await context.Set<Person>().FirstOrDefaultAsync(x => x.Rut == Rut);
        }
    }
}
