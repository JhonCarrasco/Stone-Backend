using AutoMapper;
using Microsoft.Extensions.Logging;
using Stone.Dto.Request;
using Stone.Dto.Response;
using Stone.Entities;
using Stone.Repositories.Interface;
using Stone.Services.Interface;

namespace Stone.Services.Implementation
{
    public class ProviderService : IProviderService
    {
        private readonly IProviderRepository providerRepository;
        private readonly ILogger<IProviderService> logger;
        private readonly IMapper mapper;

        public ProviderService(IProviderRepository providerRepository, ILogger<IProviderService> logger, IMapper mapper)
        {
            this.providerRepository = providerRepository;
            this.logger = logger;
            this.mapper = mapper;
        }

        public async Task<BaseResponseGeneric<int>> AddAsync(ProviderRequestDto request)
        {
            var response = new BaseResponseGeneric<int>();
            try
            {
                var providerData = await providerRepository.GetAsync(request.Id);
                if (providerData != null)
                {
                    logger.LogError(null, $"ERROR: GUARDAR Proveedor {providerData.Id}");
                    response.ErrorMessage = "Proveedor ya existe.";
                    return response;
                    //TODO: responder exception e interrumpir flujo
                }

                providerData = new Provider
                {
                    PersonId = request.PersonId,
                    LocationId = request.LocationId,
                    BankAccountId = request.BankAccountId,
                    CreateAt = DateTime.UtcNow,
                    Active = true
                };

                //for (var i=0; i<20; i++)
                //{
                //    await providerRepository.AddAsync(new Provider
                //    {
                //        PersonId = request.PersonId,
                //        LocationId = request.LocationId,
                //        BankAccountId = request.BankAccountId,
                //        CreateAt = DateTime.UtcNow,
                //        Active = true
                //    });
                //}






                var providerId = await providerRepository.AddAsync(providerData);

                response.Data = providerId;
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.ErrorMessage = "Ocurrió un error al guardar la información.";
                logger.LogError(ex, $"{response.ErrorMessage} {ex.Message}");
            }
            return response;
        }

        public async Task<BaseResponse> DeleteAsync(int id)
        {
            var response = new BaseResponse();
            try
            {
                var data = await providerRepository.GetAsync(id);
                if (data is null)
                {
                    response.ErrorMessage = $"No se ha encontrado registro con el id {id}";
                    return response;
                }

                await providerRepository.DeleteAsync(id);
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.ErrorMessage = "Ocurrió un error al obtener la información.";
                logger.LogError(ex, $"{response.ErrorMessage} {ex.Message}");
            }
            return response;
        }

        public async Task<BaseResponseGeneric<ICollection<ProviderResponseDto>>> GetAsync()
        {
            var response = new BaseResponseGeneric<ICollection<ProviderResponseDto>>();
            try
            {
                var responseProviders = await providerRepository.GetAsync();
                var dataMapper = mapper.Map<ICollection<Provider>>(responseProviders);

                response.Data = [];
                foreach (var i in dataMapper)
                {
                    var entityDto = new ProviderResponseDto
                    {
                        Id = i.Id,
                        Active = i.Active,
                        PersonName = i.Person!.DisplayName,
                        PersonRut = i.Person.Rut,
                        PersonId = i.PersonId,
                        LocationId = i.LocationId,
                        BankAccountId = i.BankAccountId,
                        CreateAt = i.CreateAt,
                        UpdatedAt = i.UpdatedAt,
                        Contacts = null
                    };
                    response.Data!.Add(entityDto);
                }
                response.Count = response.Data.Count;
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.ErrorMessage = "Ocurrió un error al obtener la información.";
                logger.LogError(ex, $"{response.ErrorMessage} {ex.Message}");
            }
            return response;
        }

        public async Task<BaseResponseGeneric<ProviderResponseDto>> GetAsync(int id)
        {
            var response = new BaseResponseGeneric<ProviderResponseDto>();
            try
            {
                var data = await providerRepository.GetAsync(id);
                var dataMapper = mapper.Map<Provider>(data);

                var entityDto = new ProviderResponseDto
                {
                    Id = dataMapper.Id,
                    Active = dataMapper.Active,
                    PersonName = dataMapper.Person!.DisplayName,
                    PersonRut = dataMapper.Person.Rut,
                    PersonId = dataMapper.PersonId,
                    LocationId = dataMapper.LocationId,
                    BankAccountId = dataMapper.BankAccountId,
                    CreateAt = dataMapper.CreateAt,
                    UpdatedAt = dataMapper.UpdatedAt,
                    Contacts = null
                };
                response.Data = entityDto;
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.ErrorMessage = "Ocurrió un error al obtener la información.";
                logger.LogError(ex, $"{response.ErrorMessage} {ex.Message}");
            }
            return response;
        }

        public async Task<BaseResponse> UpdateAsync(int id, Provider request)
        {
            var response = new BaseResponse();
            try
            {
                var data = await providerRepository.GetAsync(id);
                if (data is null)
                {
                    response.ErrorMessage = $"No se ha encontrado registro con el id {id}";
                }
                var entity = mapper.Map(request, data); //sobre escribir data nueva al objeto obtenido en la db
                await providerRepository.UpdateAsync();
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.ErrorMessage = "Ocurrió un error al actualizar la información.";
                logger.LogError(ex, $"{response.ErrorMessage} {ex.Message}");
            }
            return response;
        }
    }
}
