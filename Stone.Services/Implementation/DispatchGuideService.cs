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
    public class DispatchGuideService : IDispatchGuideService
    {
        private readonly IDispatchGuideRepository _dispatchGuideRepository;
        private readonly IMaterialRepository _materialRepository;
        private readonly ICustomerService _customerService;
        private readonly IProviderService _providerService;
        private readonly ILogger<IMaterialService> _logger;
        private readonly IMapper _mapper;

        public DispatchGuideService(IDispatchGuideRepository dispatchGuideRepository,
            IMaterialRepository materialRepository,
            ICustomerService customerService,
            IProviderService providerService,
            ILogger<IMaterialService> logger,
            IMapper mapper)
        {
            this._dispatchGuideRepository = dispatchGuideRepository;
            this._materialRepository = materialRepository;
            this._customerService = customerService;
            this._providerService = providerService;
            this._logger = logger;
            this._mapper = mapper;
        }

        public async Task<BaseResponseGeneric<int>> AddAsync(MaterialGenericRequestDto request)
        {
            var response = new BaseResponseGeneric<int>();
            try
            {

                var newDispatch = new DispatchGuide
                {
                    Folio = request.Folio,
                    DocumentType = request.DocumentType,
                    Observations = request.Observations,
                    CurrencyType = (int)request.CurrencyType,
                    ValueCurrency = request.ValueCurrency,
                    ProviderId = request.ProviderId,
                    CustomerId = request.CustomerId,
                    Neto = request.Neto,
                    TaxRate = request.TaxRate,
                    TotalValue = request.TotalValue,
                    GuideDate = request.GuideDate,
                    LocationId = request.LocationId
                    //File = request.File
                };

                var dispatchId = await _dispatchGuideRepository.AddAsync(newDispatch);


                //Inyectar listado Materiales
                if (!request.Materials.IsNullOrEmpty() && dispatchId > 0)
                {
                    foreach (var item in request.Materials!)
                    {
                        var itemMaterial = new Material
                        {
                            ProductCode = item.ProductCode,
                            Description = item.Description,
                            UnitMeasurement = item.UnitMeasurement,
                            Quantity = item.Quantity,
                            UnitValue = item.UnitValue,
                            TotalValue = item.TotalValue,
                            ProductId = item.ProductId,
                            DispatchId = dispatchId
                        };
                        var itemMaterialId = await _materialRepository.AddAsync(itemMaterial);
                    }
                }

                response.Data = dispatchId;
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.ErrorMessage = $"Ocurrió un error en {nameof(DispatchGuideService)} al guardar la información.";
                _logger.LogError(ex, $"{response.ErrorMessage} {ex.Message}");
            }
            return response;
        }

        public async Task<BaseResponse> DeleteAsync(int id)
        {
            var response = new BaseResponse();
            try
            {
                var data = await _dispatchGuideRepository.GetAsync(id);
                if (data is null)
                {
                    response.ErrorMessage = $"No se ha encontrado registro con el id {id}";
                    return response;
                }

                await _dispatchGuideRepository.DeleteAsync(id);
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.ErrorMessage = $"Ocurrió un error en {nameof(DispatchGuideService)} al obtener Guía de Despacho {id}.";
                _logger.LogError(ex, $"{response.ErrorMessage} {ex.Message}");
            }
            return response;
        }

        public async Task<BaseResponseGeneric<ICollection<MaterialResponseDto>>> GetAsync(string searchText, PaginationDto pagination)
        {
            var response = new BaseResponseGeneric<ICollection<MaterialResponseDto>>();
            try
            {
                var EntityList = await _dispatchGuideRepository.GetAsync(predicate: s => s.Observations.Contains(searchText ?? string.Empty),
                    orderBy: x => x.Id,
                    pagination);

                if (EntityList == null)
                {
                    _logger.LogError(null, $"ERROR: BUSCAR Guías de Despacho");
                    response.ErrorMessage = "Guías de Despacho no existe.";
                    return response;
                }

                response.Data = [];
                foreach (var data in EntityList)
                {
                    var EntityData = await _dispatchGuideRepository.GetAsync(data.Id);
                    if (EntityData == null)
                    {
                        _logger.LogError(null, $"ERROR: BUSCAR Guía de Despacho {data.Id}");
                        response.ErrorMessage = "Guía de Despacho no existe.";
                        return response;
                    }

                    var customerResponse = await _customerService.GetAsync((int)EntityData.CustomerId!);
                    var providerResponse = await _providerService.GetAsync((int)EntityData.ProviderId!);
                    var materialListEntity = await _materialRepository.GetByDispatchIdAsync(EntityData.Id);

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
                        GuideDate = EntityData.GuideDate,
                        LocationId = EntityData.LocationId
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
                                UnitMeasurement = item.UnitMeasurement,
                                Quantity = item.Quantity,
                                UnitValue = item.UnitValue,
                                TotalValue = item.TotalValue,
                                ProductId = item.ProductId,
                                ReceptionId = item.ReceptionId
                            };
                            materialResponse.Materials.Add(materialItem);
                        }
                    }

                    if (customerResponse.Data is not null && customerResponse.Success)
                    {
                        materialResponse.Customer = new CustomerResponseDto
                        {
                            Id = customerResponse.Data.Id,
                            Rut = customerResponse.Data.Rut,
                            Email = customerResponse.Data.Email,
                            DisplayName = customerResponse.Data.DisplayName,
                            Phone = customerResponse.Data.Phone
                        };
                    }

                    if (providerResponse.Data is not null && providerResponse.Success)
                    {
                        materialResponse.Provider = new ProviderResponseDto
                        {
                            Id = providerResponse.Data.Id,
                            Active = providerResponse.Data.Active,
                            PersonName = providerResponse.Data.PersonName,
                            PersonId = providerResponse.Data.PersonId,
                            LocationId = providerResponse.Data.LocationId,
                            BankAccountId = providerResponse.Data.BankAccountId,
                            CreateAt = providerResponse.Data.CreateAt,
                            UpdatedAt = providerResponse.Data.UpdatedAt,
                            Contacts = null
                        };
                    }

                    response.Data.Add(materialResponse);
                }
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.ErrorMessage = $"Ocurrió un error en {nameof(DispatchGuideService)} al obtener la información.";
                _logger.LogError(ex, $"{response.ErrorMessage} {ex.Message}");
            }
            return response;
        }

        public async Task<BaseResponseGeneric<MaterialResponseDto>> GetAsync(int id)
        {
            var response = new BaseResponseGeneric<MaterialResponseDto>();
            try
            {
                var EntityData = await _dispatchGuideRepository.GetAsync(id);
                if (EntityData == null)
                {
                    _logger.LogError(null, $"ERROR: BUSCAR Guía de Despacho {id}");
                    response.ErrorMessage = "Guía de Despacho no existe.";
                    return response;
                }

                var customerResponse = await _customerService.GetAsync((int)EntityData.CustomerId!);
                var providerResponse = await _providerService.GetAsync((int)EntityData.ProviderId!);
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
                    GuideDate = EntityData.GuideDate,
                    LocationId = EntityData.LocationId
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
                            UnitMeasurement = item.UnitMeasurement,
                            Quantity = item.Quantity,
                            UnitValue = item.UnitValue,
                            TotalValue = item.TotalValue,
                            ProductId = item.ProductId,
                            DispatchId = item.DispatchId
                        };
                        materialResponse.Materials.Add(materialItem);
                    }
                }

                if (customerResponse.Data is not null && customerResponse.Success)
                {
                    materialResponse.Customer = new CustomerResponseDto
                    {
                        Id = customerResponse.Data.Id,
                        Rut = customerResponse.Data.Rut,
                        Email = customerResponse.Data.Email,
                        DisplayName = customerResponse.Data.DisplayName,
                        Phone = customerResponse.Data.Phone
                    };
                }

                if (providerResponse.Data is not null && providerResponse.Success)
                {
                    materialResponse.Provider = new ProviderResponseDto
                    {
                        Id = providerResponse.Data.Id,
                        Active = providerResponse.Data.Active,
                        PersonName = providerResponse.Data.PersonName,
                        PersonId = providerResponse.Data.PersonId,
                        LocationId = providerResponse.Data.LocationId,
                        BankAccountId = providerResponse.Data.BankAccountId,
                        CreateAt = providerResponse.Data.CreateAt,
                        UpdatedAt = providerResponse.Data.UpdatedAt,
                        Contacts = null
                    };
                }

                response.Data = materialResponse;
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.ErrorMessage = $"Ocurrió un error en {nameof(DispatchGuideService)} al obtener la información.";
                _logger.LogError(ex, $"{response.ErrorMessage} {ex.Message}");
            }
            return response;
        }

        public async Task<BaseResponse> UpdateAsync(int id, MaterialGenericRequestDto request)
        {
            var response = new BaseResponseGeneric<int>();
            try
            {
                var dispatchData = await _dispatchGuideRepository.GetAsync(id);
                if (dispatchData == null)
                {
                    _logger.LogError(null, $"ERROR: ACTUALIZAR Guía de Despacho.");
                    response.ErrorMessage = "Guía de Despacho no existe.";
                    return response;
                }

                //Actualizar valores
                dispatchData.Folio = request.Folio;
                dispatchData.DocumentType = request.DocumentType;
                dispatchData.Observations = request.Observations;
                dispatchData.CurrencyType = (int)request.CurrencyType;
                dispatchData.ValueCurrency = request.ValueCurrency;
                dispatchData.ProviderId = request.ProviderId;
                dispatchData.CustomerId = request.CustomerId;
                dispatchData.Neto = request.Neto;
                dispatchData.TaxRate = request.TaxRate;
                dispatchData.TotalValue = request.TotalValue;
                dispatchData.GuideDate = request.GuideDate;
                dispatchData.LocationId = request.LocationId;
                //dispatchData.File = request.File;

                await _dispatchGuideRepository.UpdateAsync();

                //Inyectar listado Materiales
                if (!request.Materials.IsNullOrEmpty() && dispatchData.Id > 0)
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
                                UnitMeasurement = item.UnitMeasurement,
                                Quantity = item.Quantity,
                                UnitValue = item.UnitValue,
                                TotalValue = item.TotalValue,
                                ProductId = item.ProductId,
                                DispatchId = dispatchData.Id
                            };
                            var itemMaterialId = await _materialRepository.AddAsync(itemMaterial);
                        }
                        else
                        {
                            var itemMaterialData = await _materialRepository.GetAsync((int)item.Id!);
                            //Actualizar item material
                            itemMaterialData!.ProductCode = item.ProductCode;
                            itemMaterialData!.Description = item.Description;
                            itemMaterialData!.UnitMeasurement = item.UnitMeasurement;
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

                response.Data = dispatchData.Id;
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.ErrorMessage = $"Ocurrió un error en {nameof(DispatchGuideService)} al actualizar la información.";
                _logger.LogError(ex, $"{response.ErrorMessage} {ex.Message}");
            }
            return response;
        }
    }
}