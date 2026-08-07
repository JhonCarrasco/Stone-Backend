using Stone.Dto.Request;
using Stone.Dto.Response;
using Stone.Entities;

namespace Stone.Services.Interface
{
    public interface IExpenseService
    {
        Task<BaseResponseGeneric<ICollection<ExpenseResponseDto>>> GetAsync(string searchText, PaginationDto pagination);
        Task<BaseResponseGeneric<ExpenseResponseDto>> GetAsync(int id);
        Task<BaseResponseGeneric<int>> AddAsync(Expense request);
        Task<BaseResponse> UpdateAsync(int id, Expense request);
        Task<BaseResponse> DeleteAsync(int id);
    }
}
