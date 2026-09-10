using Microsoft.AspNetCore.Http;
using Stone.Dto.Request;
using Stone.Dto.Response;

namespace Stone.Services.Interface
{
    public interface IBudgetService
    {
        Task<BaseResponseGeneric<ICollection<BudgetResponseDto>>> GetAsync(string searchText, PaginationDto pagination);
        Task<BaseResponseGeneric<BudgetResponseDto>> GetAsync(int id);
        Task<BaseResponseGeneric<int>> AddAsync(BudgetRequestDto request);
        Task<BaseResponse> UpdateAsync(int id, BudgetRequestDto request);
        Task<BaseResponse> DeleteAsync(int id);
    }
}
