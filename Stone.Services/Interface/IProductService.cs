using Stone.Dto.Request;
using Stone.Dto.Response;

namespace Stone.Services.Interface
{
    public interface IProductService
    {
        Task<BaseResponseGeneric<ICollection<ProductResponseDto>>> GetAsync(string searchText, PaginationDto pagination);
        Task<BaseResponseGeneric<ProductResponseDto>> GetAsync(int id);
        Task<BaseResponseGeneric<int>> AddAsync(ProductRequestDto request);
        Task<BaseResponse> UpdateAsync(int id, ProductRequestDto request);
        Task<BaseResponse> DeleteAsync(int id);
    }
}
