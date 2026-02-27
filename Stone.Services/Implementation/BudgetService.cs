using AutoMapper;
using Microsoft.Extensions.Logging;
using Stone.Dto.Request;
using Stone.Dto.Response;
using Stone.Repositories.Interface;
using Stone.Services.Interface;

namespace Stone.Services.Implementation
{
    public class BudgetService : IBudgetService
    {
        //private readonly IBudgetRepository budgetRepository;
        private readonly ILogger<IBudgetService> logger;
        private readonly IMapper mapper;

        public BudgetService(/*IBudgetRepository budgetRepository,*/ ILogger<IBudgetService> logger, IMapper mapper)
        {
            //this.budgetRepository = budgetRepository;
            this.logger = logger;
            this.mapper = mapper;
        }

        public Task<BaseResponseGeneric<int>> AddAsync(BudgetRequestDto request)
        {
            throw new NotImplementedException();
        }

        public Task<BaseResponse> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<BaseResponseGeneric<ICollection<BudgetResponseDto>>> GetAsync()
        {
            throw new NotImplementedException();
        }

        public Task<BaseResponseGeneric<BudgetResponseDto>> GetAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<BaseResponse> UpdateAsync(int id, BudgetRequestDto request)
        {
            throw new NotImplementedException();
        }
    }
}
