using AutoMapper;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Stone.Dto.Request;
using Stone.Dto.Response;
using Stone.Entities;
using Stone.Repositories.Implementation;
using Stone.Repositories.Interface;
using Stone.Services.Interface;

namespace Stone.Services.Implementation
{
    public class ReceptionGuideService : IReceptionGuideService
    {
        private readonly IReceptionGuideRepository _receptionGuideRepository;
        private readonly ICustomerService _customerService;
        private readonly IProviderService _providerService;
        private readonly IMaterialRepository _materialRepository;
        private readonly IProductRepository _productRepository;
        private readonly ILogger<IMaterialService> _logger;
        private readonly IMapper _mapper;
        public ReceptionGuideService(IReceptionGuideRepository receptionGuideRepository,
            ICustomerService customerService,
            IProviderService providerService,
            IMaterialRepository materialRepository,
            IProductRepository productRepository,
            ILogger<IMaterialService> logger,
            IMapper mapper)
        {
            this._receptionGuideRepository = receptionGuideRepository;
            this._customerService = customerService;
            this._providerService = providerService;
            this._materialRepository = materialRepository;
            this._productRepository = productRepository;
            this._logger = logger;
            this._mapper = mapper;
        }

        public async Task<BaseResponseGeneric<int>> AddAsync(MaterialGenericRequestDto request)
        {
            var response = new BaseResponseGeneric<int>();
            try
            {

                var newReception = new ReceptionGuide
                {
                    Folio = request.Folio,
                    DocumentType = request.DocumentType,
                    Observations = request.Observations,
                    CurrencyType = request.CurrencyType,
                    ValueCurrency = request.ValueCurrency,
                    ProviderId = request.ProviderId,
                    CustomerId = request.CustomerId,
                    Neto = request.Neto,
                    TaxRate = (request.TaxRate / 100),
                    TotalValue = request.TotalValue,
                    GuideDate = request.GuideDate,
                    //File = request.File
                };

                var receptionId = await _receptionGuideRepository.AddAsync(newReception);


                //Inyectar listado Materiales
                if (!request.Materials.IsNullOrEmpty() && receptionId > 0)
                {
                    foreach (var item in request.Materials!)
                    {
                        var itemMaterial= new Material
                        {
                            ProductCode = item.ProductCode,
                            Description = item.Description,
                            UnitMeasurement = item.UnitMeasurement!.ToUpper(),
                            Quantity = item.Quantity,
                            UnitValue = item.UnitValue,
                            TotalValue = item.TotalValue,
                            ProductId = item.ProductId,
                            ReceptionId = receptionId,
                        };

                        //TODO: crear Product si no existe un productoId
                        if (item.ProductId is null)
                        {
                            var newProduct = new Product
                            {
                                Description = item.Description,
                            };

                            var newProductIdResponse = await _productRepository.AddAsync(newProduct);

                            itemMaterial.ProductId = newProductIdResponse;
                        }

                        var itemMaterialId = await _materialRepository.AddAsync(itemMaterial);
                    }
                }

                response.Data = receptionId;
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.ErrorMessage = $"Ocurrió un error en {nameof(ReceptionGuideService)} al guardar la información.";
                _logger.LogError(ex, $"{response.ErrorMessage} {ex.Message}");
            }
            return response;
        }

        public async Task<BaseResponse> DeleteAsync(int id)
        {
            var response = new BaseResponse();
            try
            {
                var data = await _receptionGuideRepository.GetAsync(id);
                if (data is null)
                {
                    response.ErrorMessage = $"No se ha encontrado registro con el id {id}";
                    return response;
                }

                await _receptionGuideRepository.DeleteAsync(id);
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.ErrorMessage = $"Ocurrió un error en {nameof(ReceptionGuideService)} al obtener Guía de Recepción {id}.";
                _logger.LogError(ex, $"{response.ErrorMessage} {ex.Message}");
            }
            return response;
        }

        public async Task<BaseResponseGeneric<ICollection<MaterialResponseDto>>> GetAsync(string searchText, PaginationDto pagination)
        {
            var response = new BaseResponseGeneric<ICollection<MaterialResponseDto>>();
            try
            {
                var EntityList = await _receptionGuideRepository.GetAsync(predicate: s => s.Observations!.Contains(searchText ?? string.Empty)
                    || s.Folio.Contains(searchText ?? string.Empty)
                    || s.Customer!.Email.Contains(searchText ?? string.Empty)
                    || s.Customer!.Person!.DisplayName.Contains(searchText ?? string.Empty)
                    || s.Customer!.Person!.Rut.Contains(searchText ?? string.Empty)
                    || s.Provider!.Person!.DisplayName.Contains(searchText ?? string.Empty)
                    || s.Provider!.Person!.Rut.Contains(searchText ?? string.Empty)
                    ,orderBy: x => x.Id
                    ,pagination);

                if (EntityList == null)
                {
                    _logger.LogError(null, $"ERROR: BUSCAR Guías de Recepción");
                    response.ErrorMessage = "Guías de Recepción no existe.";
                    return response;
                }

                response.Data = [];
                foreach (var data in EntityList)
                {
                    var EntityData = await _receptionGuideRepository.GetAsync(data.Id);
                    if (EntityData == null)
                    {
                        _logger.LogError(null, $"ERROR: BUSCAR Guía de Recepción {data.Id}");
                        response.ErrorMessage = "Guía de Recepción no existe.";
                        return response;
                    }

                    var customerMapper = _mapper.Map<Customer>(EntityData.Customer);
                    var providerMapper = _mapper.Map<Provider>(EntityData.Provider);
                    var materialListEntity = await _materialRepository.GetByReceptionIdAsync(EntityData.Id);

                    var materialResponse = new MaterialResponseDto
                    {
                        Id = EntityData.Id,
                        Active = EntityData.Active,
                        CreateAt = EntityData.CreateAt,
                        UpdatedAt = EntityData.UpdatedAt,
                        Folio = EntityData.Folio,
                        DocumentType = EntityData.DocumentType,
                        Observations = EntityData.Observations,
                        CurrencyType = EntityData.CurrencyType,
                        ValueCurrency = EntityData.ValueCurrency,
                        Neto = EntityData.Neto,
                        TaxRate = EntityData.TaxRate,
                        TotalValue = EntityData.TotalValue,
                        GuideDate = EntityData.GuideDate
                    };

                    if (materialListEntity != null && materialListEntity.Count > 0)
                    {
                        materialResponse.Materials = [];
                        foreach (var item in materialListEntity)
                        {
                            var materialItem = new Material
                            {
                                Id = item.Id,
                                Active = item.Active,
                                ProductCode = item.ProductCode,
                                Description = item.Description,
                                UnitMeasurement = item.UnitMeasurement!.ToUpper(),
                                Quantity = item.Quantity,
                                UnitValue = item.UnitValue,
                                TotalValue = item.TotalValue,
                                ProductId = item.ProductId,
                                ReceptionId = item.ReceptionId
                            };
                            materialResponse.Materials.Add(materialItem);
                        }
                    }

                    if (customerMapper is not null)
                    {
                        materialResponse.Customer = new CustomerResponseDto
                        {
                            Id = customerMapper.Id,
                            Email = customerMapper.Email,
                            Phone = customerMapper.Phone
                        };
                    }

                    if (providerMapper is not null)
                    {
                        materialResponse.Provider = new ProviderResponseDto
                        {
                            Id = providerMapper.Id,
                            Active = providerMapper.Active,
                            CreateAt = providerMapper.CreateAt,
                            UpdatedAt = providerMapper.UpdatedAt,
                            Email = providerMapper.Email,
                            Phone = providerMapper.Phone,
                            BusinessActivity = providerMapper.BusinessActivity,
                            PersonId = providerMapper.Person.Id,
                            LocationId = providerMapper.LocationId,
                            BankAccountId = providerMapper.BankAccountId,
                            
                            Person = providerMapper.Person,
                            Location = providerMapper.Location,
                            BankAccount = providerMapper.BankAccount,
                            Contacts = null //TODO: Implementar mapeo de contactos si es necesario
                        };
                    }

                    response.Data.Add(materialResponse);
                }
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.ErrorMessage = $"Ocurrió un error en {nameof(ReceptionGuideService)} al obtener la información.";
                _logger.LogError(ex, $"{response.ErrorMessage} {ex.Message}");
            }
            return response;
        }

        public async Task<BaseResponseGeneric<MaterialResponseDto>> GetAsync(int id)
        {
            var response = new BaseResponseGeneric<MaterialResponseDto>();
            try
            {
                var EntityData = await _receptionGuideRepository.GetAsync(id);
                if (EntityData == null)
                {
                    _logger.LogError(null, $"ERROR: BUSCAR Guía de Recepción {id}");
                    response.ErrorMessage = "Guía de Recepción no existe.";
                    return response;
                }

                var customerMapper = _mapper.Map<Customer>(EntityData.Customer);
                var providerMapper = _mapper.Map<Provider>(EntityData.Provider);
                var materialListEntity = await _materialRepository.GetAsync(predicate: s => s.ReceptionId == id,
                    orderBy: x => x.Id,
                    pagination: new PaginationDto { OffSet = 0, Limit = 100 });

                var materialResponse = new MaterialResponseDto
                {
                    Id = EntityData.Id,
                    Active = EntityData.Active,
                    CreateAt = EntityData.CreateAt,
                    UpdatedAt = EntityData.UpdatedAt,
                    Folio = EntityData.Folio,
                    DocumentType = EntityData.DocumentType,
                    Observations = EntityData.Observations,
                    CurrencyType = EntityData.CurrencyType,
                    ValueCurrency = EntityData.ValueCurrency,
                    Neto = EntityData.Neto,
                    TaxRate = EntityData.TaxRate,
                    TotalValue = EntityData.TotalValue,
                    GuideDate = EntityData.GuideDate
                };

                if (materialListEntity is not null && materialListEntity.Count > 0)
                {
                    materialResponse.Materials = [];
                    foreach (var item in materialListEntity)
                    {
                        var materialItem = new Material
                        {
                            Id = item.Id,
                            Active = item.Active,
                            ProductCode = item.ProductCode,
                            Description = item.Description,
                            UnitMeasurement = item.UnitMeasurement!.ToUpper(),
                            Quantity = item.Quantity,
                            UnitValue = item.UnitValue,
                            TotalValue = item.TotalValue,
                            ProductId = item.ProductId,
                            ReceptionId = item.ReceptionId
                        };
                        materialResponse.Materials.Add(materialItem);
                    }
                }

                if (customerMapper is not null)
                {
                    materialResponse.Customer = new CustomerResponseDto
                    {
                        Id = customerMapper.Id,
                        Email = customerMapper.Email,
                        Phone = customerMapper.Phone
                    };
                }

                if (providerMapper is not null)
                {
                    materialResponse.Provider = new ProviderResponseDto
                    {
                        Id = providerMapper.Id,
                        Active = providerMapper.Active,
                        CreateAt = providerMapper.CreateAt,
                        UpdatedAt = providerMapper.UpdatedAt,
                        Email = providerMapper.Email,
                        Phone = providerMapper.Phone,
                        BusinessActivity = providerMapper.BusinessActivity,
                        PersonId = providerMapper.Person.Id,
                        LocationId = providerMapper.LocationId,
                        BankAccountId = providerMapper.BankAccountId,

                        Person = providerMapper.Person,
                        Location = providerMapper.Location,
                        BankAccount = providerMapper.BankAccount,
                        Contacts = null //TODO: Implementar mapeo de contactos si es necesario
                    };
                }

                response.Data = materialResponse;
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.ErrorMessage = $"Ocurrió un error en {nameof(ReceptionGuideService)} al obtener la información.";
                _logger.LogError(ex, $"{response.ErrorMessage} {ex.Message}");
            }
            return response;
        }

        public async Task<BaseResponse> UpdateAsync(int id, MaterialGenericRequestDto request)
        {
            var response = new BaseResponseGeneric<int>();
            try
            {
                var receptionData = await _receptionGuideRepository.GetAsync(id);
                if (receptionData == null)
                {
                    _logger.LogError(null, $"ERROR: ACTUALIZAR Guía de Recepción.");
                    response.ErrorMessage = "Guía de Recepción no existe.";
                    return response;
                }

                //Actualizar valores
                receptionData.UpdatedAt = DateTime.UtcNow;
                receptionData.Folio = request.Folio;
                receptionData.DocumentType = request.DocumentType;
                receptionData.Observations = request.Observations;
                receptionData.CurrencyType = request.CurrencyType;
                receptionData.ValueCurrency = request.ValueCurrency;
                receptionData.ProviderId = request.ProviderId;
                receptionData.CustomerId = request.CustomerId;
                receptionData.Neto = request.Neto;
                receptionData.TaxRate = (request.TaxRate / 100);
                receptionData.TotalValue = request.TotalValue;
                receptionData.GuideDate = request.GuideDate;
                //receptionData.File = request.File;

                await _receptionGuideRepository.UpdateAsync();

                //Inyectar listado Materiales
                if (!request.Materials.IsNullOrEmpty() && receptionData.Id > 0)
                {
                    foreach (var item in request.Materials!)
                    {
                        if (item.Id == null || item.Id == 0)
                        {
                            //guardar item material
                            var itemMaterial = new Material
                            {
                                ProductCode = item.ProductCode,
                                Description = item.Description,
                                UnitMeasurement = item.UnitMeasurement!.ToUpper(),
                                Quantity = item.Quantity,
                                UnitValue = item.UnitValue,
                                TotalValue = item.TotalValue,
                                ProductId = item.ProductId,
                                ReceptionId = receptionData.Id
                            };

                            //TODO: crear Product si no existe un productoId
                            if (item.ProductId is null)
                            {
                                var newProduct = new Product
                                {
                                    Description = item.Description,
                                };

                                var newProductIdResponse = await _productRepository.AddAsync(newProduct);

                                itemMaterial.ProductId = newProductIdResponse;
                            }

                            var itemMaterialId = await _materialRepository.AddAsync(itemMaterial);
                        }
                        else
                        {
                            var itemMaterialData = await _materialRepository.GetAsync((int)item.Id!);
                            //if (itemMaterialData is not null && !item.Active ){
                            //    await _materialRepository.DeleteAsync(itemMaterialData.Id);
                            //}

                            //Actualizar item material
                            itemMaterialData!.ProductCode = item.ProductCode;
                            itemMaterialData!.Description = item.Description;
                            itemMaterialData!.UnitMeasurement = item.UnitMeasurement!.ToUpper();
                            itemMaterialData!.Quantity = item.Quantity;
                            itemMaterialData!.UnitValue = item.UnitValue;
                            itemMaterialData!.TotalValue = item.TotalValue;
                            itemMaterialData!.ProductId = item.ProductId;

                            itemMaterialData.Active = item.Active;
                            itemMaterialData.CreateAt = item.CreateAt;
                            itemMaterialData.UpdatedAt = DateTime.UtcNow;

                            await _materialRepository.UpdateAsync();
                        }
                    }
                }

                response.Data = receptionData.Id;
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.ErrorMessage = $"Ocurrió un error en {nameof(ReceptionGuideService)} al actualizar la información.";
                _logger.LogError(ex, $"{response.ErrorMessage} {ex.Message}");
            }
            return response;
        }
    }
}
