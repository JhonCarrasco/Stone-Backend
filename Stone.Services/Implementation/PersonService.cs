using AutoMapper;
using Microsoft.Extensions.Logging;
using Stone.Dto.Response;
using Stone.Entities;
using Stone.Repositories.Interface;
using Stone.Services.Interface;

namespace Stone.Services.Implementation
{
    public class PersonService : IPersonService
    {
        private readonly IPersonRepository repository;
        private readonly ILogger<PersonService> logger;
        private readonly IMapper mapper;

        public PersonService(IPersonRepository repository, ILogger<PersonService> logger, IMapper mapper)
        {
            this.repository = repository;
            this.logger = logger;
            this.mapper = mapper;
        }

        public async Task<BaseResponseGeneric<int>> AddAsync(Person person)
        {
            var response = new BaseResponseGeneric<int>();

            try
            {
                response.Data = await repository.AddAsync(person); ;
                response.Success = true;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "{ErrorMessage} {Message}", response.ErrorMessage, ex.Message);
            }

            return response;
        }
    }
}
