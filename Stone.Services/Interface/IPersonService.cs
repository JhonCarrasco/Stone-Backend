using Stone.Dto.Response;
using Stone.Entities;

namespace Stone.Services.Interface
{
    public interface IPersonService
    {
        Task<BaseResponseGeneric<int>> AddAsync(Person person);
    }
}
