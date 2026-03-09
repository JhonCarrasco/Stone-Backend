using Stone.Entities;

namespace Stone.Repositories.Interface
{
    public interface IPersonRepository : IRepositoryBase<Person>
    {
        Task<Person?> GetByRutAsync(string rut);
    }
}
