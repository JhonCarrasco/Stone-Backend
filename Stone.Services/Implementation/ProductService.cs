using AutoMapper;
using Microsoft.Extensions.Logging;
using Stone.Dto.Request;
using Stone.Dto.Response;
using Stone.Entities;
using Stone.Repositories.Interface;
using Stone.Services.Interface;

namespace Stone.Services.Implementation
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository productRepository;
        private readonly IManufacturerRepository manufacturerRepository;
        private readonly ICategoryRepository categoryRepository;
        private readonly ILogger<ICustomerService> logger;
        private readonly IMapper mapper;

        public ProductService(IProductRepository productRepository,
            IManufacturerRepository manufacturerRepository,
            ICategoryRepository categoryRepository,
            ILogger<ICustomerService> logger,
            IMapper mapper)
        {
            this.productRepository = productRepository;
            this.manufacturerRepository = manufacturerRepository;
            this.categoryRepository = categoryRepository;
            this.logger = logger;
            this.mapper = mapper;
        }

        public async Task<BaseResponseGeneric<ICollection<ProductResponseDto>>> GetAsync()
        {
            var response = new BaseResponseGeneric<ICollection<ProductResponseDto>>();
            try
            {
                var productEntityList = await productRepository.GetAsync();
                if (productEntityList == null)
                {
                    logger.LogError(null, $"ERROR: BUSCAR Productos");
                    response.ErrorMessage = "Productos no existe.";
                    return response;
                    //TODO: responder exception e interrumpir flujo
                }

                response.Data = [];
                foreach (var data in productEntityList)
                {
                    var manufacturerMapper = mapper.Map<Manufacturer>(data.Manufacturer);
                    var categoryMapper = mapper.Map<Category>(data.Category);
                    var providerMapper = mapper.Map<Provider>(data.Provider);
                    var contactsMapper = mapper.Map<List<Contact>>(data.Provider!.Contacts);

                    var productResponse = new ProductResponseDto
                    {
                        Id = data.Id,
                        Active = data.Active,
                        CreateAt = data.CreateAt,
                        UpdatedAt = DateTime.UtcNow,
                        Description = data.Description,
                        Long = data.Long,
                        Width = data.Width,
                        Thickness = data.Thickness,
                        Color = data.Color,
                        UnitMeasurement = data.UnitMeasurement,
                        UnitValue = data.UnitValue,
                        Manufacturer = manufacturerMapper,
                        Category = categoryMapper
                    };
                    //Mapear provider
                    var providerResponse = new ProviderResponseDto
                    {
                        Id = providerMapper.Id,
                        Active = providerMapper.Active,
                        CreateAt = providerMapper.CreateAt,
                        UpdatedAt = providerMapper.UpdatedAt,
                        PersonId = providerMapper.PersonId,
                        LocationId = providerMapper.LocationId,
                        BankAccountId = providerMapper.BankAccountId
                    };

                    if (contactsMapper is not null)
                    {
                        var contactsList = new List<ContactResponseDto>();
                        foreach (var itemContact in contactsMapper)
                        {
                            var contactsResponse = new ContactResponseDto
                            {
                                ContactId = itemContact.Id,
                                BusinessActivity = itemContact.BusinessActivity,
                                Phone = itemContact.Phone,
                                Email = itemContact.Email,
                                PersonId = itemContact.PersonId,
                                Rut = itemContact.Person!.Rut,
                                DisplayName = itemContact.Person.DisplayName,
                                ProviderId = itemContact.Provider!.Id,
                                ProviderName = itemContact.Provider.Person.DisplayName
                            };
                            contactsList.Add(contactsResponse);

                        }
                       providerResponse.Contacts = contactsList;
                    }
                    productResponse.Provider = providerResponse;
                    response.Data.Add(productResponse);
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

        public async Task<BaseResponseGeneric<ProductResponseDto>> GetAsync(int id)
        {
            var response = new BaseResponseGeneric<ProductResponseDto>();
            try
            {
                var productData = await productRepository.GetAsync(id);
                if (productData == null)
                {
                    logger.LogError(null, $"ERROR: BUSCAR Producto {id}");
                    response.ErrorMessage = "Producto no existe.";
                    return response;
                    //TODO: responder exception e interrumpir flujo
                }

                var manufacturerMapper = mapper.Map<Manufacturer>(productData.Manufacturer);
                var categoryMapper = mapper.Map<Category>(productData.Category);
                var providerMapper = mapper.Map<Provider>(productData.Provider);
                var contactsMapper = mapper.Map<List<Contact>>(productData.Provider!.Contacts);

                var productResponse = new ProductResponseDto
                {
                    Id = productData.Id,
                    Active = productData.Active,
                    CreateAt = productData.CreateAt,
                    UpdatedAt = DateTime.UtcNow,
                    Description = productData.Description,
                    Long = productData.Long,
                    Width = productData.Width,
                    Thickness = productData.Thickness,
                    Color = productData.Color,
                    UnitMeasurement = productData.UnitMeasurement,
                    UnitValue = productData.UnitValue,
                    Manufacturer = manufacturerMapper,
                    Category = categoryMapper
                };
                //Mapear provider
                var providerResponse = new ProviderResponseDto
                {
                    Id = providerMapper.Id,
                    Active = providerMapper.Active,
                    CreateAt = providerMapper.CreateAt,
                    UpdatedAt = providerMapper.UpdatedAt,
                    PersonId = providerMapper.PersonId,
                    LocationId = providerMapper.LocationId,
                    BankAccountId = providerMapper.BankAccountId
                };

                if (contactsMapper is not null)
                {
                    var contactsList = new List<ContactResponseDto>();
                    foreach (var itemContact in contactsMapper)
                    {
                        var contactsResponse = new ContactResponseDto
                        {
                            ContactId = itemContact.Id,
                            BusinessActivity = itemContact.BusinessActivity,
                            Phone = itemContact.Phone,
                            Email = itemContact.Email,
                            PersonId = itemContact.PersonId,
                            Rut = itemContact.Person!.Rut,
                            DisplayName = itemContact.Person.DisplayName,
                            ProviderId = itemContact.Provider!.Id,
                            ProviderName = itemContact.Provider.Person.DisplayName
                        };
                        contactsList.Add(contactsResponse);

                    }
                    providerResponse.Contacts = contactsList;
                }
                productResponse.Provider = providerResponse;
                response.Data = productResponse;
                
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.ErrorMessage = "Ocurrió un error al obtener la información.";
                logger.LogError(ex, $"{response.ErrorMessage} {ex.Message}");
            }
            return response;
        }

        public async Task<BaseResponseGeneric<int>> AddAsync(ProductRequestDto request)
        {
            var response = new BaseResponseGeneric<int>();
            try
            {
                var productData = await productRepository.GetByDescAsync(request.Description);
                if (productData != null)
                {
                    logger.LogError(null, $"ERROR: GUARDAR Producto {productData.Id}");
                    response.ErrorMessage = "Producto ya existe.";
                    return response;
                    //TODO: responder exception e interrumpir flujo
                }

                productData = new Product
                {
                    Description         = request.Description,
                    Long                = request.Long,
                    Width               = request.Width,
                    Thickness           = request.Thickness,
                    Color               = request.Color,
                    UnitMeasurement     = request.UnitMeasurement,
                    UnitValue           = request.UnitValue,
                    ManufacturerId      = (int)request.ManufacturerId,
                    CategoryId          = request.CategoryId,
                    ProviderId          = request.ProviderId
                };

                var productId = await productRepository.AddAsync(productData);

                response.Data = productId;
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.ErrorMessage = "Ocurrió un error al guardar la información.";
                logger.LogError(ex, $"{response.ErrorMessage} {ex.Message}");
            }
            return response;
        }

        public async Task<BaseResponse> UpdateAsync(int id, ProductRequestDto request)
        {
            var response = new BaseResponseGeneric<int>();
            try
            {
                var productData = await productRepository.GetAsync(id);

                //var productData = await productRepository.GetByDescAsync(request.Description);
                if (productData == null)
                {
                    logger.LogError(null, $"ERROR: Actualizar Producto: {productData.Description}");
                    response.ErrorMessage = "Producto no existe.";
                    return response;
                    //TODO: responder exception e interrumpir flujo
                }

                //Asignar valores a actualizar
                productData.Description     = request.Description;
                productData.Long            = request.Long;
                productData.Width           = request.Width;
                productData.Thickness       = request.Thickness;
                productData.Color           = request.Color;
                productData.UnitMeasurement = request.UnitMeasurement;
                productData.UnitValue       = request.UnitValue;
                productData.ManufacturerId  = (int)request.ManufacturerId;
                productData.CategoryId      = request.CategoryId;
                productData.ProviderId      = request.ProviderId;                
                await productRepository.UpdateAsync();

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
                var data = await productRepository.GetAsync(id);
                if (data is null)
                {
                    response.ErrorMessage = $"No se ha encontrado registro con el id {id}";
                    return response;
                }

                await productRepository.DeleteAsync(id);
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.ErrorMessage = $"Ocurrió un error al obtener Producto {id}.";
                logger.LogError(ex, $"{response.ErrorMessage} {ex.Message}");
            }
            return response;
        }
    }
}
