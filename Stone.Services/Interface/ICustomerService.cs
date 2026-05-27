using Stone.Dto.Request;
using Stone.Dto.Response;

namespace Stone.Services.Interface
{
    public interface ICustomerService
    {
        Task<BaseResponseGeneric<ICollection<CustomerResponseDto>>> GetAsync(string searchText, PaginationDto pagination);
        Task<BaseResponseGeneric<CustomerResponseDto>> GetAsync(int id);
        Task<BaseResponseGeneric<int>> AddAsync(CustomerRequestDto request);
        Task<BaseResponse> UpdateAsync(int id, CustomerRequestDto request);
        Task<BaseResponse> DeleteAsync(int id);
    }
}
