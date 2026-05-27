using Stone.Dto.Request;
using Stone.Dto.Response;
using Stone.Entities;

namespace Stone.Services.Interface
{
    public interface IProviderService
    {
        Task<BaseResponseGeneric<ICollection<ProviderResponseDto>>> GetAsync();
        Task<BaseResponseGeneric<ProviderResponseDto>> GetAsync(int id);
        Task<BaseResponseGeneric<int>> AddAsync(ProviderRequestDto request);
        Task<BaseResponse> UpdateAsync(int id, Provider request);
        Task<BaseResponse> DeleteAsync(int id);
    }
}
