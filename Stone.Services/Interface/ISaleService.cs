using Stone.Dto.Request;
using Stone.Dto.Response;

namespace Stone.Services.Interface
{
    public interface ISaleService
    {
        Task<BaseResponseGeneric<int>> AddAsync(string email, SaleRequestDto request);
        Task<BaseResponseGeneric<SaleResponseDto>> GetAsync(int id);
        Task<BaseResponseGeneric<ICollection<SaleResponseDto>>> GetAsync(SaleByDateSearchDto search, PaginationDto pagination);
        Task<BaseResponseGeneric<ICollection<SaleResponseDto>>> GetAsync(string email, string title, PaginationDto pagination);
        Task<BaseResponseGeneric<ICollection<SaleReportResponseDto>>> GetSaleReportAsync(DateTime dateStart, DateTime dateEnd);
    }
}
