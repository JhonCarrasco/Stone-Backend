using Stone.Dto.Request;
using Stone.Dto.Response;

namespace Stone.Services.Interface
{
    public interface IDispatchGuideService
    {
        Task<BaseResponseGeneric<ICollection<MaterialResponseDto>>> GetAsync(string searchText, PaginationDto pagination);
        Task<BaseResponseGeneric<MaterialResponseDto>> GetAsync(int id);
        Task<BaseResponseGeneric<int>> AddAsync(MaterialGenericRequestDto request);
        Task<BaseResponse> UpdateAsync(int id, MaterialGenericRequestDto request);
        Task<BaseResponse> DeleteAsync(int id);
    }
}
