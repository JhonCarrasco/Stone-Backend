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

        public async Task<BaseResponseGeneric<ICollection<CustomerResponseDto>>> GetAsync()
        {
            var response = new BaseResponseGeneric<ICollection<CustomerResponseDto>>();
            try
            {
                var customerEntityList = await customerRepository.GetAsync();
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
                        Rut = data.Rut,
                        Email = data.Email,
                        DisplayName = data.DisplayName,
                        Phone = data.Phone,
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
                    Rut = data.Rut,
                    Email = data.Email,
                    DisplayName = data.DisplayName,
                    Phone = data.Phone,
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
                    TypePerson = request.TypePerson,
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
                    Rut = request.Rut,
                    Email = request.Email,
                    DisplayName = request.DisplayName!,
                    Phone = request.Phone!,
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
                            Account = request.AccountNumber,
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
                            BusinessActivity = itemContact.BusinessActivity,
                            Phone = itemContact.Phone,
                            Email = itemContact.Email,
                            CustomerId = customerId,
                            ProviderId = itemContact.ProviderId,
                        };

                        //Guardar personas si no existen
                        var personContactEntity = await personRepository.GetByRutAsync(itemContact.Rut);
                        var personEntityId = 0;
                        if (personContactEntity == null)
                        {
                            //Guardar nueva persona id en entity Contacto
                            personEntityId = await personRepository.AddAsync(new Person
                            {
                                Rut = itemContact.Rut,
                                DisplayName = itemContact.DisplayName,
                                TypePerson = itemContact.TypePerson
                            });
                            contact.PersonId = personEntityId;
                        }

                        //TODO: preguntar si personaId, clienteId, proveedorId, ya existe en entidad contacto antes de guardar, evitar duplicidad

                        contact.PersonId = personContactEntity != null ? personContactEntity.Id : personEntityId;
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
                    personData.TypePerson = request.TypePerson;
                    personData.Active = personData.Active;
                    personData.CreateAt = personData.CreateAt;
                    personData.UpdatedAt = DateTime.UtcNow;

                    await personRepository.UpdateAsync();
                    //var entity = mapper.Map(personEntity, personData); //sobre escribir data nueva al objeto obtenido en la db
                    //await personRepository.UpdateAsync();
                    #endregion

                    //Actualizar Cliente entity
                    customerData.Id = customerData.Id;
                    customerData.Rut = request.Rut;
                    customerData.Email = request.Email;
                    customerData.DisplayName = request.DisplayName!;
                    customerData.Phone = request.Phone!;
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
                            bankAccountData.Account = request.AccountNumber!;
                            bankAccountData.TypeAccountId = (int)request.TypeAccountId!;                            
                            await bankAccountRepository.UpdateAsync();
                        }
                        else
                        {
                            //Guardar informacion de banco
                            var bankAccount = new BankAccount
                            {
                                BankId = (int)request.BankId!,
                                Account = request.AccountNumber!,
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
                            if (itemContact.ContactId is not null)
                            {
                                //buscar contacto por id 
                                var contactData = await contactRepository.GetAsync((int)itemContact.ContactId);
                                if (contactData is not null)
                                {
                                    //Actualizar contacto entity
                                    contactData.BusinessActivity = itemContact.BusinessActivity;
                                    contactData.Phone = itemContact.Phone;
                                    contactData.Email = itemContact.Email;
                                    contactData.ProviderId = itemContact.ProviderId;
                                    await contactRepository.UpdateAsync();
                                }                                

                                //Actualizar persona
                                var personContactData = await personRepository.GetAsync((int)itemContact.PersonId!);
                                if(personContactData is not null)
                                {
                                    personContactData.Rut = itemContact.Rut;
                                    personContactData.DisplayName = itemContact.DisplayName;
                                    personContactData.TypePerson = itemContact.TypePerson;
                                    personContactData.UpdatedAt = DateTime.UtcNow;
                                    await personRepository.UpdateAsync();
                                }
                            }
                            else
                            {
                                //Crear contacto entity
                                var contact = new Contact
                                {
                                    BusinessActivity = itemContact.BusinessActivity,
                                    Phone = itemContact.Phone,
                                    Email = itemContact.Email,
                                    CustomerId = customerData.Id,
                                    ProviderId = itemContact.ProviderId,
                                    CreateAt = DateTime.UtcNow
                                };

                                //Guardar personas si no existen
                                var personContactEntity = await personRepository.GetByRutAsync(itemContact.Rut);
                                var personEntityId = 0;
                                if (personContactEntity == null)
                                {
                                    //Guardar nueva persona id en entity Contacto
                                    personEntityId = await personRepository.AddAsync(new Person
                                    {
                                        Rut = itemContact.Rut,
                                        DisplayName = itemContact.DisplayName,
                                        TypePerson = itemContact.TypePerson
                                    });
                                    contact.PersonId = personEntityId;
                                }

                                contact.PersonId = personContactEntity != null ? personContactEntity.Id : personEntityId;
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
