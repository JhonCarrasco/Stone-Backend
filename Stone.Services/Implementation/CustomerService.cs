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
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository customerRepository;
        private readonly IPersonRepository personRepository;
        private readonly IBankAccountRepository bankAccountRepository;
        private readonly ILocationRepository locationRepository;
        private readonly IContactRepository contactRepository;        
        private readonly ILogger<ICustomerService> logger;
        private readonly IMapper mapper;

        public CustomerService(
            ICustomerRepository customerRepository, 
            IPersonRepository personRepository,
            IBankAccountRepository bankAccountRepository,
            ILocationRepository locationRepository,
            IContactRepository contactRepository,
            ILogger<ICustomerService> logger,
            IMapper mapper)
        {
            this.customerRepository = customerRepository;
            this.personRepository = personRepository;
            this.bankAccountRepository = bankAccountRepository;
            this.locationRepository = locationRepository;
            this.contactRepository = contactRepository;
            this.logger = logger;
            this.mapper = mapper;
        }

        public async Task<BaseResponseGeneric<ICollection<CustomerResponseDto>>> GetAsync(string? searchText, PaginationDto pagination)
        {
            var response = new BaseResponseGeneric<ICollection<CustomerResponseDto>>();
            try
            {
                var customerEntityList = await customerRepository.GetAsync(
                    predicate: s => s.Person!.DisplayName.Contains(searchText ?? string.Empty) 
                    || s.Person.Rut.Contains(searchText ?? string.Empty),
                    orderBy: x => x.Person!.DisplayName,
                    pagination);
                //response.Data = mapper.Map<ICollection<Customer>>(customerEntity);
                response.Data = [];
                foreach (var data in customerEntityList)
                {
                    var personaMapper = mapper.Map<Person>(data.Person);
                    var locationMapper = mapper.Map<Location>(data.Location);
                    var bankAccountMapper = mapper.Map<BankAccount>(data.BankAccount);
                    var contactsMapper = mapper.Map<List<Contact>>(data.Contacts);

                    var customerResponse = new CustomerResponseDto
                    {
                        Id = data.Id,
                        Active = data.Active,
                        CreateAt = data.CreateAt,
                        UpdatedAt = data.UpdatedAt,
                        Email = data.Email,
                        Phone = data.Phone,                        
                        BusinessActivity = data.BusinessActivity,
                        Person = personaMapper,
                        Location = locationMapper,
                        BankAccount = bankAccountMapper
                    };

                    if (contactsMapper is not null)
                    {
                        customerResponse.Contacts = [];
                        foreach (var itemContact in contactsMapper)
                        {
                            var contactsResponse = new ContactResponseDto
                            {
                                Id = itemContact.Id,
                                Active = itemContact.Active,
                                Phone = itemContact.Phone,
                                Email = itemContact.Email,
                                DisplayName = itemContact.DisplayName,
                                Position = itemContact.Position,
                                CustomerId = itemContact.CustomerId,
                            };
                            customerResponse.Contacts.Add(contactsResponse);
                        }
                    }

                    response.Data.Add(customerResponse);
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

        public async Task<BaseResponseGeneric<CustomerResponseDto>> GetAsync(int id)
        {
            var response = new BaseResponseGeneric<CustomerResponseDto>();
            
            try
            {
                var data = await customerRepository.GetAsync(id);

                var personaMapper = mapper.Map<Person>(data.Person);
                var locationMapper = mapper.Map<Location>(data.Location);
                var bankAccountMapper = mapper.Map<BankAccount>(data.BankAccount);
                var contactsMapper = mapper.Map<List<Contact>>(data.Contacts);

                var customerResponse = new CustomerResponseDto
                {
                    Id = data.Id,
                    Active = data.Active,
                    CreateAt = data.CreateAt,
                    UpdatedAt = data.UpdatedAt,
                    Email = data.Email,
                    Phone = data.Phone,
                    BusinessActivity = data.BusinessActivity,
                    Person = personaMapper,
                    Location = locationMapper,
                    BankAccount = bankAccountMapper
                };

                if (contactsMapper is not null)
                {
                    customerResponse.Contacts = [];
                    foreach (var itemContact in contactsMapper)
                    {
                        var contactsResponse = new ContactResponseDto
                        {
                            Id = itemContact.Id,
                            Active = itemContact.Active,                            
                            Phone = itemContact.Phone,
                            Email = itemContact.Email,
                            DisplayName = itemContact.DisplayName,
                            Position = itemContact.Position,
                            CustomerId = itemContact.CustomerId,
                        };
                        customerResponse.Contacts.Add(contactsResponse);
                    }
                }

                response.Data = customerResponse;
                response.Success = response.Data != null;
            }
            catch (Exception ex)
            {
                response.ErrorMessage = "Ocurrió un error al obtener la información.";
                logger.LogError(ex, $"{response.ErrorMessage} {ex.Message}");
            }
            return response;
        }       

        public async Task<BaseResponseGeneric<int>> AddAsync(CustomerRequestDto request)
        {
            var response = new BaseResponseGeneric<int>();
            try
            {
                #region Cliente - Persona
                //Buscar Persona si existe
                var personEntity = await personRepository.GetByRutAsync(request.Rut);
                if (personEntity != null)
                {
                    logger.LogError(null, $"ERROR: GUARDAR PERSONA {request.Rut}");
                    response.ErrorMessage = "Cliente(persona) ya existe.";
                    return response;
                    //TODO: responder exception e interrumpir flujo
                }

                //Guardar Persona entity
                personEntity = new Person
                {
                    Rut = request.Rut,
                    DisplayName = request.DisplayName,
                    TypePerson = request.TypePersonId,
                };

                var personaId = await personRepository.AddAsync(personEntity);

                if (personaId == 0)
                {
                    logger.LogError(null, $"ERROR: GUARDAR PERSONA {request.Rut}");
                    response.ErrorMessage = "ERROR: GUARDAR PERSONA {request.Rut}.";
                    return response;
                    //TODO: responder exception e interrumpir flujo
                }

                //Crear Cliente entity
                var customerEntity = new Customer
                {
                    Email = request.Email,
                    Phone = request.Phone!,
                    BusinessActivity = request.BusinessActivity,
                    PersonId = personaId,
                    LocationId = null,
                    BankAccountId = null
                };
                #endregion

                #region Cuenta Banco
                if (request.BankId != null || !request.AccountNumber.IsNullOrEmpty() || request.TypeAccountId != null)
                {
                    var bankAccountEntity = await bankAccountRepository.GetAsync(request.AccountNumber!, (int)request.BankId!, (int)request.TypeAccountId!);
                    if (bankAccountEntity == null)
                    {
                        //Guardar informacion de banco
                        var bankAccount = new BankAccount
                        {
                            BankId = (int)request.BankId!,
                            AccountNumber = request.AccountNumber,
                            TypeAccountId = (int)request.TypeAccountId!
                        };
                        var bankAccountId = await bankAccountRepository.AddAsync(bankAccount);
                        //Asignar id en entidad cliente
                        customerEntity.BankAccountId = bankAccountId;
                    }
                }
                #endregion

                #region Ubicacion
                if (!request.Address.IsNullOrEmpty() || request.RegionId != null || request.CommuneId != null)
                {
                    var locationEntity = await locationRepository.GetAsync(request.Address!, (int)request.CommuneId!, (int)request.RegionId!);
                    if (locationEntity == null)
                    {
                        //Guardar informacion de ubicacion
                        var location = new Location
                        {
                            Address = request.Address,
                            RegionId = (int)request.RegionId!,
                            CommuneId = (int)request.CommuneId!
                        };
                        var locationId = await locationRepository.AddAsync(location);
                        //Asignar id en entidad cliente
                        customerEntity.LocationId = locationId;
                    }

                }
                #endregion

                var customerId = await customerRepository.AddAsync(customerEntity); ;

                #region Contacto
                if (!request.Contacts.IsNullOrEmpty() && customerId > 0)
                {
                    //Cliente ya debe tener id
                    foreach (var itemContact in request.Contacts!)
                    {
                        //Crear contacto entity
                        var contact = new Contact
                        {
                            CreateAt = DateTime.UtcNow,
                            DisplayName = itemContact.DisplayName,
                            Phone = itemContact.Phone,
                            Email = itemContact.Email,
                            Position = itemContact.Position,
                            CustomerId = customerId,
                        };

                        ////Guardar personas si no existen
                        //var personContactEntity = await personRepository.GetByRutAsync(itemContact.Phone);
                        //var personEntityId = 0;
                        //if (personContactEntity == null)
                        //{
                        //    //Guardar nueva persona id en entity Contacto
                        //    personEntityId = await personRepository.AddAsync(new Person
                        //    {
                        //        Rut = itemContact.Rut,
                        //        DisplayName = itemContact.DisplayName,
                        //        TypePerson = itemContact.TypePerson
                        //    });
                        //    contact.PersonId = personEntityId;
                        //}
                        //TODO: preguntar si personaId, clienteId, proveedorId, ya existe en entidad contacto antes de guardar, evitar duplicidad
                        //contact.PersonId = personContactEntity != null ? personContactEntity.Id : personEntityId;

                        //Guardar listado de contactos
                        var contactRespose = await contactRepository.AddAsync(contact);
                    }
                }
                #endregion

                response.Data = customerId;
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.ErrorMessage = "Ocurrió un error al guardar la información.";
                logger.LogError(ex, $"{response.ErrorMessage} {ex.Message}");
            }
            return response;
        }

        public async Task<BaseResponse> UpdateAsync(int id, CustomerRequestDto request)
        {
            var response = new BaseResponse();
            try
            {
                //buscar si existe cliente
                var customerData = await customerRepository.GetAsync(id);

                if(customerData is not null)
                {
                    #region Persona
                    //Buscar Persona si existe
                    var personData = await personRepository.GetAsync(customerData.Person!.Id);
                    if (personData is null)
                    {
                        logger.LogError(null, $"ERROR: Actualizar PERSONA {request.Rut}");
                        response.ErrorMessage = "Cliente(persona) no existe.";
                        return response;
                        //TODO: responder exception e interrumpir flujo
                    }

                    //Actualizar Persona entity
                    personData.Rut = request.Rut;
                    personData.DisplayName = request.DisplayName;
                    personData.TypePerson = request.TypePersonId;
                    personData.Active = personData.Active;
                    personData.CreateAt = personData.CreateAt;
                    personData.UpdatedAt = DateTime.UtcNow;

                    await personRepository.UpdateAsync();
                    #endregion

                    //Actualizar Cliente entity
                    customerData.Id = customerData.Id;
                    customerData.Email = request.Email;
                    customerData.Phone = request.Phone!;
                    customerData.BusinessActivity = request.BusinessActivity;
                    customerData.PersonId = customerData.PersonId;
                    customerData.LocationId = customerData.LocationId;
                    customerData.BankAccountId = customerData.BankAccountId;
                    customerData.UpdatedAt = DateTime.UtcNow;

                    #region Cuenta Banco
                    if (request.BankId != null || !request.AccountNumber.IsNullOrEmpty() || request.TypeAccountId != null)
                    {
                        var bankAccountData = await bankAccountRepository.GetAsync((int)customerData.BankAccountId!);
                        if(bankAccountData is not null)
                        {
                            //Actualizar informacion de banco
                            bankAccountData.BankId = (int)request.BankId!;
                            bankAccountData.AccountNumber = request.AccountNumber!;
                            bankAccountData.TypeAccountId = (int)request.TypeAccountId!;                            
                            await bankAccountRepository.UpdateAsync();
                        }
                        else
                        {
                            //Guardar informacion de banco
                            var bankAccount = new BankAccount
                            {
                                BankId = (int)request.BankId!,
                                AccountNumber = request.AccountNumber!,
                                TypeAccountId = (int)request.TypeAccountId!
                            };
                            var bankAccountId = await bankAccountRepository.AddAsync(bankAccount);
                            //Asignar id en entidad cliente
                            customerData.BankAccountId = bankAccountId;
                        }
                    }
                    #endregion
                    
                    #region Ubicacion
                    if (!request.Address.IsNullOrEmpty() || request.RegionId != null || request.CommuneId != null)
                    {
                        var locationData = await locationRepository.GetAsync((int)customerData.LocationId!);
                        if (locationData is not null)
                        {
                            //Actualizar informacion de banco
                            locationData.Address = request.Address;
                            locationData.RegionId = (int)request.RegionId!;
                            locationData.CommuneId = (int)request.CommuneId!;                            
                            await locationRepository.UpdateAsync();
                        }
                        else
                        {
                            //Guardar informacion de ubicacion
                            var location = new Location
                            {
                                Address = request.Address,
                                RegionId = (int)request.RegionId!,
                                CommuneId = (int)request.CommuneId!
                            };
                            var locationId = await locationRepository.AddAsync(location);
                            //Asignar id en entidad cliente
                            customerData.LocationId = locationId;
                        }
                    }
                    #endregion

                    await customerRepository.UpdateAsync();

                    #region Contacto
                    if (!request.Contacts.IsNullOrEmpty())
                    {                      
                        foreach (var itemContact in request.Contacts!)
                        {                       
                            if (itemContact.Id is not null)
                            {
                                //buscar contacto por id 
                                var contactData = await contactRepository.GetAsync((int)itemContact.Id);
                                if (contactData is not null)
                                {
                                    //Actualizar contacto entity
                                    contactData.DisplayName = itemContact.DisplayName;                                    
                                    contactData.Phone = itemContact.Phone;
                                    contactData.Email = itemContact.Email;
                                    contactData.Position = itemContact.Position;
                                    contactData.ProviderId = itemContact.ProviderId;
                                    contactData.UpdatedAt = DateTime.UtcNow;
                                    await contactRepository.UpdateAsync();
                                }                               
                            }
                            else
                            {
                                //Crear contacto entity
                                var contact = new Contact
                                {
                                    DisplayName = itemContact.DisplayName,
                                    Phone = itemContact.Phone,
                                    Email = itemContact.Email,
                                    Position = itemContact.Position,
                                    CustomerId = customerData.Id,
                                    CreateAt = DateTime.UtcNow
                                };                                                               
                                //Guardar listado de contactos
                                await contactRepository.AddAsync(contact);
                            }
                        }
                    }
                    #endregion

                    response.Success = true;
                }
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
                var data = await customerRepository.GetAsync(id);
                if (data is null)
                {
                    response.ErrorMessage = $"No se ha encontrado registro con el id {id}";
                    return response;
                }

                await customerRepository.DeleteAsync(id);
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.ErrorMessage = $"Ocurrió un error al obtener Cliente {id}.";
                logger.LogError(ex, $"{response.ErrorMessage} {ex.Message}");
            }
            return response;
        }
    }
}
