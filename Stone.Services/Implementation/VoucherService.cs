using AutoMapper;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Stone.Dto.Request;
using Stone.Dto.Response;
using Stone.Entities;
using Stone.Repositories.Interface;
using Stone.Services.Interface;

namespace Stone.Services.Implementation
{
    public class VoucherService : IVoucherService
    {
        private readonly IMaterialVoucherRepository _voucherRepository;
        private readonly IMaterialRepository _materialRepository;
        private readonly ILogger<IMaterialService> _logger;
        private readonly IMapper _mapper;

        public VoucherService(IMaterialVoucherRepository voucherRepository,
            IMaterialRepository materialRepository,
            ILogger<IMaterialService> logger,
            IMapper mapper)
        {
            _voucherRepository = voucherRepository;
            _materialRepository = materialRepository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<BaseResponseGeneric<ICollection<MaterialResponseDto>>> GetAsync(string searchText, PaginationDto pagination)
        {
            var response = new BaseResponseGeneric<ICollection<MaterialResponseDto>>();
            try
            {
                var EntityList = await _voucherRepository.GetAsync(predicate: s => s.Observations!.Contains(searchText ?? string.Empty)
                    || s.SupplierTo.Contains(searchText ?? string.Empty)
                    || s.ProjectTo!.Contains(searchText ?? string.Empty)
                    || s.Budget!.Customer!.Person!.DisplayName.Contains(searchText ?? string.Empty)
                    || s.Budget!.Description!.Contains(searchText ?? string.Empty)
                    ,orderBy: x => x.Id
                    ,pagination);

                if (EntityList == null)
                {
                    _logger.LogError(null, $"ERROR: BUSCAR Vales de consumo");
                    response.ErrorMessage = "Vales de consumo no existe.";
                    return response;
                }

                response.Data = [];
                foreach (var data in EntityList)
                {
                    var EntityData = await _voucherRepository.GetAsync(data.Id);
                    if (EntityData == null)
                    {
                        _logger.LogError(null, $"ERROR: BUSCAR Vale de consumo {data.Id}");
                        response.ErrorMessage = "Vale de consumo no existe.";
                        return response;
                    }

                    var materialListEntity = await _materialRepository.GetByReceptionIdAsync(EntityData.Id);

                    var materialResponse = new MaterialResponseDto
                    {
                        Id = EntityData.Id,
                        Active = EntityData.Active,
                        CreateAt = EntityData.CreateAt,
                        UpdatedAt = EntityData.UpdatedAt,
                        Observations = EntityData.Observations,
                        SupplierTo = EntityData.SupplierTo,
                        ProjectTo = EntityData.ProjectTo,
                        BudgetId = EntityData.BudgetId
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

                    //if (customerResponse.Data is not null && customerResponse.Success)
                    //{
                    //    materialResponse.Customer = new CustomerResponseDto
                    //    {
                    //        Id = customerResponse.Data.Id,
                    //        Rut = customerResponse.Data.Rut,
                    //        Email = customerResponse.Data.Email,
                    //        DisplayName = customerResponse.Data.DisplayName,
                    //        Phone = customerResponse.Data.Phone
                    //    };
                    //}

                    //if (providerResponse.Data is not null && providerResponse.Success)
                    //{
                    //    materialResponse.Provider = new ProviderResponseDto
                    //    {
                    //        Id = providerResponse.Data.Id,
                    //        Active = providerResponse.Data.Active,
                    //        PersonName = providerResponse.Data.PersonName,
                    //        PersonId = providerResponse.Data.PersonId,
                    //        LocationId = providerResponse.Data.LocationId,
                    //        BankAccountId = providerResponse.Data.BankAccountId,
                    //        CreateAt = providerResponse.Data.CreateAt,
                    //        UpdatedAt = providerResponse.Data.UpdatedAt,
                    //        Contacts = null
                    //    };
                    //}

                    response.Data.Add(materialResponse);
                }
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.ErrorMessage = $"Ocurrió un error en {nameof(VoucherService)} al obtener la información.";
                _logger.LogError(ex, $"{response.ErrorMessage} {ex.Message}");
            }
            return response;
        }

        public async Task<BaseResponseGeneric<MaterialResponseDto>> GetAsync(int id)
        {
            var response = new BaseResponseGeneric<MaterialResponseDto>();
            try
            {
                var EntityData = await _voucherRepository.GetAsync(id);
                if (EntityData == null)
                {
                    _logger.LogError(null, $"ERROR: BUSCAR Comprobante {id}");
                    response.ErrorMessage = "Comprobante no existe.";
                    return response;
                }

                var budgetMapper = _mapper.Map<Budget>(EntityData.Budget);
                var materialListEntity = await _materialRepository.GetAsync(predicate: s => s.VoucherId == id
                    ,orderBy: x => x.Id
                    ,pagination: new PaginationDto { OffSet = 0, Limit = 100 });

                var materialResponse = new MaterialResponseDto
                {
                    Id = EntityData.Id,
                    Active = EntityData.Active,
                    CreateAt = EntityData.CreateAt,
                    UpdatedAt = EntityData.UpdatedAt,
                    Observations = EntityData.Observations,
                    SupplierTo = EntityData.SupplierTo,
                    ProjectTo = EntityData.ProjectTo,
                    BudgetId = EntityData.BudgetId,
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
                            ReceptionId = item.ReceptionId
                        };
                        materialResponse.Materials.Add(materialItem);
                    }
                }

                if (budgetMapper is not null)
                {
                    materialResponse.Budget = new BudgetResponseDto
                    {
                        Id = budgetMapper.Id,
                        Active = budgetMapper.Active,
                        CreateAt = budgetMapper.CreateAt,
                        UpdatedAt = budgetMapper.UpdatedAt,
                        ProjectName = budgetMapper.ProjectName,
                        Address = budgetMapper.Address,
                        Description = budgetMapper.Description,
                        Material = budgetMapper.Material,
                        SubTotal = budgetMapper.SubTotal,
                        Neto = budgetMapper.Neto,
                        TaxRate = budgetMapper.TaxRate,
                        TotalValue = budgetMapper.TotalValue,
                        CustomerId = budgetMapper.CustomerId,
                        State = budgetMapper.State,
                        Zone = budgetMapper.Zone,
                        ContactPerson = budgetMapper.ContactPerson,
                        PhoneContact = budgetMapper.PhoneContact
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

        public async Task<BaseResponseGeneric<int>> AddAsync(MaterialGenericRequestDto request)
        {
            var response = new BaseResponseGeneric<int>();
            try
            {

                var newVoucher = new MaterialVoucher
                {
                    Observations = request.Observations,
                    SupplierTo = request.SupplierTo!,
                    ProjectTo = request.ProjectTo,
                    BudgetId = request.BudgetId
                };

                var voucherId = await _voucherRepository.AddAsync(newVoucher);


                //Inyectar listado Materiales
                if (!request.Materials.IsNullOrEmpty() && voucherId > 0)
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
                            VoucherId = voucherId
                        };
                        var itemMaterialId = await _materialRepository.AddAsync(itemMaterial);
                    }
                }

                response.Data = voucherId;
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.ErrorMessage = $"Ocurrió un error en {nameof(VoucherService)} al guardar la información.";
                _logger.LogError(ex, $"{response.ErrorMessage} {ex.Message}");
            }
            return response;
        }

        public async Task<BaseResponse> UpdateAsync(int id, MaterialGenericRequestDto request)
        {
            var response = new BaseResponseGeneric<int>();
            try
            {
                var voucherData = await _voucherRepository.GetAsync(id);
                if (voucherData == null)
                {
                    _logger.LogError(null, $"ERROR: ACTUALIZAR Comprobante de Material.");
                    response.ErrorMessage = "Comprobante de Material no existe.";
                    return response;
                }

                //Actualizar valores
                voucherData.Observations = request.Observations;
                voucherData.SupplierTo = request.SupplierTo!;
                voucherData.ProjectTo = request.ProjectTo;
                voucherData.BudgetId = request.BudgetId;
                voucherData.UpdatedAt = DateTime.UtcNow;

                await _voucherRepository.UpdateAsync();

                //Inyectar listado Materiales
                if (!request.Materials.IsNullOrEmpty() && voucherData.Id > 0)
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
                                VoucherId = voucherData.Id
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

                response.Data = voucherData.Id;
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.ErrorMessage = $"Ocurrió un error en {nameof(VoucherService)} al actualizar la información.";
                _logger.LogError(ex, $"{response.ErrorMessage} {ex.Message}");
            }
            return response;
        }

        public async Task<BaseResponse> DeleteAsync(int id)
        {
            var response = new BaseResponse();
            try
            {
                var data = await _voucherRepository.GetAsync(id);
                if (data is null)
                {
                    response.ErrorMessage = $"No se ha encontrado registro con el id {id}";
                    return response;
                }

                await _voucherRepository.DeleteAsync(id);
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.ErrorMessage = $"Ocurrió un error en {nameof(VoucherService)} al obtener Comprobante {id}.";
                _logger.LogError(ex, $"{response.ErrorMessage} {ex.Message}");
            }
            return response;
        }
    }
}