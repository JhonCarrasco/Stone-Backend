using Stone.Dto.Response;
using Stone.Entities.Generic;

namespace Stone.Services.Interface
{
    public interface IPersonService
    {
        Task<BaseResponseGeneric<int>> AddAsync(Person person);
    }
}
