using Microsoft.EntityFrameworkCore;
using Stone.Entities.Generic;
using Stone.Persistence;
using Stone.Repositories.Implements;
using Stone.Repositories.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
