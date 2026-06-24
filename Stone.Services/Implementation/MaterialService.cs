using AutoMapper;
using Microsoft.Extensions.Logging;
using Stone.Dto.Request;
using Stone.Dto.Response;
using Stone.Entities;
using Stone.Repositories.Implementation;
using Stone.Repositories.Interface;
using Stone.Services.Interface;

namespace Stone.Services.Implementation
{
    public class MaterialService : IMaterialService
    {
        public MaterialService()
        {
        }

        public async Task<BaseResponseGeneric<int>> AddAsync(MaterialGenericRequestDto request)
        {
            throw new NotImplementedException();
        }

        public Task<BaseResponse> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<BaseResponseGeneric<ICollection<MaterialResponseDto>>> GetAsync(string searchText, PaginationDto pagination)
        {
            throw new NotImplementedException();
        }

        public Task<BaseResponseGeneric<MaterialResponseDto>> GetAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<BaseResponse> UpdateAsync(int id, MaterialGenericRequestDto request)
        {
            throw new NotImplementedException();
        }
    }
}
