using AutoMapper;
using Microsoft.Extensions.Logging;
using Stone.Dto.Response;
using Stone.Entities;
using Stone.Repositories.Interface;
using Stone.Services.Interface;

namespace Stone.Services.Implementation
{
    public class SharedService : ISharedService
    {
        private readonly IBankRepository bankRepository;
        private readonly ICategoryRepository categoryRepository;
        private readonly ICommuneRepository communeRepository;
        private readonly IManufacturerRepository manufacturerRepository;
        private readonly IRegionRepository regionRepository;
        private readonly ITypeAccountRepository typeAccountRepository;
        private readonly IUnitMeasurementRepository unitMeasurementRepository;
        private readonly ILogger<ISharedService> logger;
        private readonly IMapper mapper;

        public SharedService(IBankRepository bankRepository, ICategoryRepository categoryRepository, ICommuneRepository communeRepository 
                            , IManufacturerRepository manufacturerRepository, IRegionRepository regionRepository
                            , ITypeAccountRepository typeAccountRepository, IUnitMeasurementRepository unitMeasurementRepository
                            , ILogger<ISharedService> logger, IMapper mapper)
        {
            this.bankRepository = bankRepository;
            this.categoryRepository = categoryRepository;
            this.communeRepository = communeRepository;
            this.manufacturerRepository = manufacturerRepository;
            this.regionRepository = regionRepository;
            this.typeAccountRepository = typeAccountRepository;
            this.unitMeasurementRepository = unitMeasurementRepository;
            this.logger = logger;
            this.mapper = mapper;
        }

        public async Task<BaseResponseGeneric<ICollection<Bank>>> GetBankAsync()
        {
            var response = new BaseResponseGeneric<ICollection<Bank>>();
            try
            {
                response.Data = mapper.Map<ICollection<Bank>>(await bankRepository.GetAsync());
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

        public async Task<BaseResponseGeneric<ICollection<Category>>> GetCategoryAsync()
        {
            var response = new BaseResponseGeneric<ICollection<Category>>();
            try
            {
                response.Data = mapper.Map<ICollection<Category>>(await categoryRepository.GetAsync());
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

        public async Task<BaseResponseGeneric<ICollection<Commune>>> GetCommuneAsync()
        {
            var response = new BaseResponseGeneric<ICollection<Commune>>();
            try
            {
                response.Data = mapper.Map<ICollection<Commune>>(await communeRepository.GetAsync());
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

        public async Task<BaseResponseGeneric<ICollection<Manufacturer>>> GetManufacturerAsync()
        {
            var response = new BaseResponseGeneric<ICollection<Manufacturer>>();
            try
            {
                response.Data = mapper.Map<ICollection<Manufacturer>>(await manufacturerRepository.GetAsync());
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

        public async Task<BaseResponseGeneric<ICollection<Region>>> GetRegionAsync()
        {
            var response = new BaseResponseGeneric<ICollection<Region>>();
            try
            {
                response.Data = mapper.Map<ICollection<Region>>(await regionRepository.GetAsync());
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

        public async Task<BaseResponseGeneric<ICollection<TypeAccount>>> GetTypeAccountAsync()
        {
            var response = new BaseResponseGeneric<ICollection<TypeAccount>>();
            try
            {
                response.Data = mapper.Map<ICollection<TypeAccount>>(await typeAccountRepository.GetAsync());
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

        public async Task<BaseResponseGeneric<ICollection<UnitMeasurement>>> GetUnitMeasurementAsync()
        {
            var response = new BaseResponseGeneric<ICollection<UnitMeasurement>>();
            try
            {
                response.Data = mapper.Map<ICollection<UnitMeasurement>>(await unitMeasurementRepository.GetAsync());
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
    }
}
