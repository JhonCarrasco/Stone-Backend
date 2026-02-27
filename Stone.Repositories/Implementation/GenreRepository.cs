using Stone.Entities;
using Stone.Persistence;
using Stone.Repositories.Interface;

namespace Stone.Repositories.Implements
{
    public class GenreRepository : RepositoryBase<Genre>, IGenreRepository
    {
        public GenreRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
