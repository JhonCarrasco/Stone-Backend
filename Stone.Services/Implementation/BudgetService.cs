using AutoMapper;
using Azure.Core;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Stone.Dto.Request;
using Stone.Dto.Response;
using Stone.Entities;
using Stone.Repositories.Implementation;
using Stone.Repositories.Interface;
using Stone.Services.Interface;
using System.Drawing;
using System.Runtime.InteropServices;

namespace Stone.Services.Implementation
{
    public class BudgetService : IBudgetService
    {
        private readonly IBudgetRepository budgetRepository;
        private readonly IItemizedProductRepository itemizedProductRepository;
        private readonly IItemizedServiceRepository itemizedServiceRepository;
        private readonly ILogger<IBudgetService> logger;
        private readonly IMapper mapper;

        public BudgetService(
            IBudgetRepository budgetRepository, 
            IItemizedProductRepository itemizedProductRepository, 
            IItemizedServiceRepository itemizedServiceRepository,
            ILogger<IBudgetService> logger, 
            IMapper mapper)
        {
            this.budgetRepository = budgetRepository;
            this.itemizedProductRepository = itemizedProductRepository;
            this.itemizedServiceRepository = itemizedServiceRepository;
            this.logger = logger;
            this.mapper = mapper;
        }

        public async Task<BaseResponseGeneric<ICollection<BudgetResponseDto>>> GetAsync(string? searchText, PaginationDto pagination)
        {
            var response = new BaseResponseGeneric<ICollection<BudgetResponseDto>>();
            try
            {
                var EntityList = await budgetRepository.GetAsync(predicate: s => s.Description.Contains(searchText ?? string.Empty),
                    orderBy: x => x.Description,
                    pagination);

                if (EntityList == null)
                {
                    logger.LogError(null, $"ERROR: BUSCAR Presupuestos");
                    response.ErrorMessage = "Presupuestos no existe.";
                    return response;
                    //TODO: responder exception e interrumpir flujo
                }

                response.Data = [];
                foreach (var data in EntityList)
                {
                    var customerMapper = mapper.Map<Customer>(data.Customer);
                    var ItemizedProductsMapper = mapper.Map<ICollection<ItemizedProduct>>(data.ItemizedProducts);
                    var ItemizedServicesMapper = mapper.Map<ICollection<ItemizedService>>(data.ItemizedServices);

                    var budgetResponse = new BudgetResponseDto
                    {
                        Id = data.Id,
                        Active = data.Active,
                        CreateAt = data.CreateAt,
                        UpdatedAt = data.UpdatedAt,
                        ProjectName = data.ProjectName,
                        Address = data.Address,
                        Description = data.Description,
                        Material = data.Material,
                        SubTotal = data.SubTotal,
                        Neto = data.Neto,
                        TaxRate = data.TaxRate,
                        TotalValue = data.TotalValue,
                        CustomerId = data.CustomerId,
                        State = data.State,
                        Zone = data.Zone,
                        ContactPerson = data.ContactPerson,
                        PhoneContact = data.PhoneContact
                    };

                    if (customerMapper is not null)
                    {
                        budgetResponse.Customer = new CustomerResponseDto
                        {
                            Id = customerMapper.Id,
                            Rut = customerMapper.Rut,
                            Email = customerMapper.Email,
                            DisplayName = customerMapper.DisplayName,
                            Phone = customerMapper.Phone                            
                        };
                    }

                    if (ItemizedProductsMapper is not null)
                    {
                        budgetResponse.ItemizedProducts = [];
                        foreach (var item in ItemizedProductsMapper)
                        {
                            var itemizedProductResponse = new ItemizedProductResponseDto
                            {
                                Id = item.Id,
                                Active = item.Active,
                                CreateAt = item.CreateAt,
                                UpdatedAt = item.UpdatedAt,
                                Description = item.Description,
                                Long = item.Long,
                                Width = item.Width,
                                Thickness = item.Thickness,
                                Color = item.Color,
                                Material = item.Material,
                                UnitValue = item.UnitValue,
                                amount = item.amount,
                                TotalValue = item.TotalValue,
                                BudgetId = (int)item.BudgetId!
                            };
                            budgetResponse.ItemizedProducts.Add(itemizedProductResponse);
                        }
                    }

                    if (ItemizedServicesMapper is not null)
                    {
                        budgetResponse.ItemizedServices = [];
                        foreach (var item in ItemizedServicesMapper)
                        {
                            var itemizedServiceResponse = new ItemizedServiceResponseDto
                            {
                                Id = item.Id,
                                Active = item.Active,
                                CreateAt = item.CreateAt,
                                UpdatedAt = item.UpdatedAt,
                                Description = item.Description,
                                UnitValue = item.UnitValue,
                                amount = item.amount,
                                TotalValue = item.TotalValue,
                                BudgetId = (int)item.BudgetId!
                            };
                            budgetResponse.ItemizedServices.Add(itemizedServiceResponse);
                        }
                    }

                    response.Data.Add(budgetResponse);
                }
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.ErrorMessage = "Ocurrió un error al obtener la información.";
                logger.LogError(ex, $"{response.ErrorMessage} {ex.Message}");
            }
            return response;
        }

        public async Task<BaseResponseGeneric<BudgetResponseDto>> GetAsync(int id)
        {
            var response = new BaseResponseGeneric<BudgetResponseDto>();
            try
            {
                var EntityData = await budgetRepository.GetAsync(id);
                if (EntityData == null)
                {
                    logger.LogError(null, $"ERROR: BUSCAR Presupuesto {id}");
                    response.ErrorMessage = "Presupuesto no existe.";
                    return response;
                    //TODO: responder exception e interrumpir flujo
                }

                var customerMapper = mapper.Map<Customer>(EntityData.Customer);
                var ItemizedProductsMapper = mapper.Map<ICollection<ItemizedProduct>>(EntityData.ItemizedProducts);
                var ItemizedServicesMapper = mapper.Map<ICollection<ItemizedService>>(EntityData.ItemizedServices);

                var budgetResponse = new BudgetResponseDto
                {
                    Id = EntityData.Id,
                    Active = EntityData.Active,
                    CreateAt = EntityData.CreateAt,
                    UpdatedAt = EntityData.UpdatedAt,
                    ProjectName = EntityData.ProjectName,
                    Address = EntityData.Address,
                    Description = EntityData.Description,
                    Material = EntityData.Material,
                    SubTotal = EntityData.SubTotal,
                    Neto = EntityData.Neto,
                    TaxRate = EntityData.TaxRate,
                    TotalValue = EntityData.TotalValue,
                    CustomerId = EntityData.CustomerId,
                    State = EntityData.State,
                    Zone = EntityData.Zone,
                    ContactPerson = EntityData.ContactPerson,
                    PhoneContact = EntityData.PhoneContact
                };

                    if (customerMapper is not null)
                    {
                        budgetResponse.Customer = new CustomerResponseDto
                        {
                            Id = customerMapper.Id,
                            Rut = customerMapper.Rut,
                            Email = customerMapper.Email,
                            DisplayName = customerMapper.DisplayName,
                            Phone = customerMapper.Phone
                        };
                    }

                    if (ItemizedProductsMapper is not null)
                    {
                        budgetResponse.ItemizedProducts = [];
                        foreach (var item in ItemizedProductsMapper)
                        {
                            var itemizedProductResponse = new ItemizedProductResponseDto
                            {
                                Id = item.Id,
                                Active = item.Active,
                                CreateAt = item.CreateAt,
                                UpdatedAt = item.UpdatedAt,
                                Description = item.Description,
                                Long = item.Long,
                                Width = item.Width,
                                Thickness = item.Thickness,
                                Color = item.Color,
                                Material = item.Material,
                                UnitValue = item.UnitValue,
                                amount = item.amount,
                                TotalValue = item.TotalValue,
                                BudgetId = (int)item.BudgetId!
                            };
                            budgetResponse.ItemizedProducts.Add(itemizedProductResponse);
                        }
                    }

                    if (ItemizedServicesMapper is not null)
                    {
                        budgetResponse.ItemizedServices = [];
                        foreach (var item in ItemizedServicesMapper)
                        {
                            var itemizedServiceResponse = new ItemizedServiceResponseDto
                            {
                                Id = item.Id,
                                Active = item.Active,
                                CreateAt = item.CreateAt,
                                UpdatedAt = item.UpdatedAt,
                                Description = item.Description,
                                UnitValue = item.UnitValue,
                                amount = item.amount,
                                TotalValue = item.TotalValue,
                                BudgetId = (int)item.BudgetId!
                            };
                            budgetResponse.ItemizedServices.Add(itemizedServiceResponse);
                        }
                    }

                response.Data = budgetResponse;                
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.ErrorMessage = "Ocurrió un error al obtener la información.";
                logger.LogError(ex, $"{response.ErrorMessage} {ex.Message}");
            }
            return response;
        }

        public async Task<BaseResponseGeneric<int>> AddAsync(BudgetRequestDto request)
        {
            var response = new BaseResponseGeneric<int>();
            try
            {
                var budgetData = new Budget
                {
                    ProjectName = request.ProjectName,
                    Address = request.Address,
                    Description = request.Description,
                    Material = request.Material,
                    SubTotal = request.SubTotal,
                    Neto = request.Neto,
                    TaxRate = (request.TaxRate/100), //porcentaje Iva en decimal hacia DB
                    TotalValue = request.TotalValue,
                    CustomerId = request.CustomerId,
                    State = request.State,
                    Zone = request.Zone,
                    ContactPerson = request.ContactPerson,
                    PhoneContact = request.PhoneContact
                };

                var budgetId = await budgetRepository.AddAsync(budgetData);

                //Inyectar listado Productos
                if (!request.ItemizedProducts.IsNullOrEmpty() && budgetId > 0)
                {
                    foreach (var item in request.ItemizedProducts!)
                    {
                        var itemProducto = new ItemizedProduct
                        {
                            Description = item.Description,
                            Long = item.Long,
                            Width = item.Width,
                            Thickness = item.Thickness,
                            Color = item.Color,
                            Material = item.Material,
                            UnitValue = item.UnitValue,
                            amount = item.amount,
                            TotalValue = item.TotalValue,
                            BudgetId = budgetId
                        };
                        var itemProductoId = await itemizedProductRepository.AddAsync(itemProducto);
                    }
                }

                //Inyectar listado de Servicios
                if (!request.ItemizedServices.IsNullOrEmpty() && budgetId > 0)
                {
                    foreach (var item in request.ItemizedServices!)
                    {
                        var itemService = new ItemizedService
                        {
                            Description = item.Description,
                            UnitValue = item.UnitValue,
                            amount = item.amount,
                            TotalValue = item.TotalValue,
                            BudgetId = budgetId
                        };
                        var itemServiceId = await itemizedServiceRepository.AddAsync(itemService);
                    }
                }

                response.Data = budgetId;
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.ErrorMessage = "Ocurrió un error al guardar la información.";
                logger.LogError(ex, $"{response.ErrorMessage} {ex.Message}");
            }
            return response;
        }

        public async Task<BaseResponse> UpdateAsync(int id, BudgetRequestDto request)
        {
            var response = new BaseResponseGeneric<int>();
            try
            {
                var budgetData = await budgetRepository.GetAsync(id);
                if (budgetData == null)
                {
                    logger.LogError(null, $"ERROR: ACTUALIZAR Presupuesto.");
                    response.ErrorMessage = "Presupuesto no existe.";
                    return response;
                    //TODO: responder exception e interrumpir flujo
                }

                //Setear valores
                budgetData.ProjectName  = request.ProjectName;
                budgetData.Address      = request.Address;
                budgetData.Description  = request.Description;
                budgetData.Material     = request.Material;
                budgetData.SubTotal     = request.SubTotal;
                budgetData.Neto         = request.Neto;
                budgetData.TaxRate = (request.TaxRate / 100); //porcentaje Iva en decimal hacia DB
                budgetData.TotalValue   = request.TotalValue;
                budgetData.CustomerId   = request.CustomerId;
                budgetData.UpdatedAt    = DateTime.UtcNow;     
                budgetData.State = request.State;
                budgetData.Zone = request.Zone;
                budgetData.ContactPerson = request.ContactPerson;
                budgetData.PhoneContact = request.PhoneContact;
                
                await budgetRepository.UpdateAsync();

                //Inyectar listado Productos
                if (!request.ItemizedProducts.IsNullOrEmpty() && budgetData.Id > 0)
                {
                    foreach (var item in request.ItemizedProducts!)
                    {                        
                        if (item.Id == null || item.Id == 0)
                        {
                            //guardar item producto
                            var itemProducto = new ItemizedProduct
                            {
                                Description = item.Description,
                                Long = item.Long,
                                Width = item.Width,
                                Thickness = item.Thickness,
                                Color = item.Color,
                                Material = item.Material,
                                UnitValue = item.UnitValue,
                                amount = item.amount,
                                TotalValue = item.TotalValue,
                                BudgetId = budgetData.Id
                            };
                            var itemProductoId = await itemizedProductRepository.AddAsync(itemProducto);
                        }
                        else
                        {
                            var itemProductoData = await itemizedProductRepository.GetAsync((int)item.Id!);
                            //Actualizar item producto
                            itemProductoData!.Description = item.Description;
                            itemProductoData.Long = item.Long;
                            itemProductoData.Width = item.Width;
                            itemProductoData.Thickness = item.Thickness;
                            itemProductoData.Color = item.Color;
                            itemProductoData.Material = item.Material;
                            itemProductoData.UnitValue = item.UnitValue;
                            itemProductoData.amount = item.amount;
                            itemProductoData.TotalValue = item.TotalValue;
                            //itemProductoData.BudgetId = item.BudgetId;
                            itemProductoData.Active = item.Active;
                            itemProductoData.CreateAt = item.CreateAt;
                            itemProductoData.UpdatedAt = DateTime.UtcNow;

                            await itemizedProductRepository.UpdateAsync();
                        }                          
                    }
                }

                //Inyectar listado de Servicios
                if (!request.ItemizedServices.IsNullOrEmpty() && budgetData.Id > 0)
                {
                    foreach (var item in request.ItemizedServices!)
                    {                        
                        if (item.Id == null || item.Id == 0)
                        {
                            //guardar item servicio
                            var itemService = new ItemizedService
                            {
                                Description = item.Description,
                                UnitValue = item.UnitValue,
                                amount = item.amount,
                                TotalValue = item.TotalValue,
                                BudgetId = budgetData.Id
                            };
                            var itemServiceId = await itemizedServiceRepository.AddAsync(itemService);
                        }
                        else
                        {
                            var itemizedServiceData = await itemizedServiceRepository.GetAsync((int)item.Id!);
                            //Actualizar item servicio
                            itemizedServiceData!.Description = item.Description;
                            itemizedServiceData.UnitValue = item.UnitValue;
                            itemizedServiceData.amount = item.amount;
                            itemizedServiceData.TotalValue = item.TotalValue;
                            //itemizedServiceData.BudgetId = item.BudgetId;
                            itemizedServiceData.Active = item.Active;
                            itemizedServiceData.CreateAt = item.CreateAt;
                            itemizedServiceData.UpdatedAt = DateTime.UtcNow;
                            await itemizedServiceRepository.UpdateAsync();
                        }
                    }
                }

                response.Data = budgetData.Id;
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.ErrorMessage = "Ocurrió un error al actualizar la información.";
                logger.LogError(ex, $"{response.ErrorMessage} {ex.Message}");
            }
            return response;
        }

        public async Task<BaseResponse> DeleteAsync(int id)
        {
            var response = new BaseResponse();
            try
            {
                var data = await budgetRepository.GetAsync(id);
                if (data is null)
                {
                    response.ErrorMessage = $"No se ha encontrado registro con el id {id}";
                    return response;
                }

                await budgetRepository.DeleteAsync(id);
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.ErrorMessage = $"Ocurrió un error al obtener Presupuesto {id}.";
                logger.LogError(ex, $"{response.ErrorMessage} {ex.Message}");
            }
            return response;
        }
    }
}
