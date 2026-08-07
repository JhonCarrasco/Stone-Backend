using AutoMapper;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Stone.Dto.Request;
using Stone.Dto.Response;
using Stone.Entities;
using Stone.Repositories.Implementation;
using Stone.Repositories.Interface;
using Stone.Services.Interface;
using System.ComponentModel;

namespace Stone.Services.Implementation
{
    public class ExpenseService : IExpenseService
    {
        private readonly IExpenseRepository _expenseRepository;
        private readonly ILogger<IMaterialService> _logger;
        private readonly IMapper _mapper;

        public ExpenseService(
            IExpenseRepository expenseRepository,
            ILogger<IMaterialService> logger,
            IMapper mapper)
        {
            this._expenseRepository = expenseRepository;
            this._logger = logger;
            this._mapper = mapper;
        }


        public async Task<BaseResponseGeneric<ICollection<ExpenseResponseDto>>> GetAsync(string searchText, PaginationDto pagination)
        {
            var response = new BaseResponseGeneric<ICollection<ExpenseResponseDto>>();
            try
            {
                var EntityList = await _expenseRepository.GetAsync(predicate: s => 
                    s.Amount.ToString()!.Contains(searchText ?? string.Empty)
                    || s.Project.Contains(searchText ?? string.Empty)
                    || s.Employee!.Contains(searchText ?? string.Empty)
                    || s.Description!.Contains(searchText ?? string.Empty)
                    //|| s.Budget!.Customer!.Person!.DisplayName.Contains(searchText ?? string.Empty)
                    //|| s.Budget!.Description!.Contains(searchText ?? string.Empty)
                    , orderBy: x => x.Id
                    , pagination);

                //response.Data = _mapper.Map<ICollection<Expense>>(EntityList);
                if (EntityList == null)
                {
                    _logger.LogError(null, $"ERROR: BUSCAR Gastos");
                    response.ErrorMessage = "Gastos no existen.";
                    return response;
                }

                response.Data = [];
                foreach (var data in EntityList)
                {
                    var expenseResponse = new ExpenseResponseDto
                    {
                        Id = data.Id,
                        Active = data.Active,
                        CreateAt = data.CreateAt,
                        UpdatedAt = data.UpdatedAt,
                        ExpenseTypeId = data.ExpenseTypeId,
                        ExpenseDate = data.ExpenseDate,
                        Employee = data.Employee,
                        Project = data.Project,
                        Description = data.Description,
                        BudgetId = data.BudgetId,
                        File = data.File,
                        Amount = data.Amount,
                        MethodPaymentId = data.MethodPaymentId,
                        PaymentReceiptId = data.PaymentReceiptId,
                        BillNumber = data.BillNumber,
                        Vehicle = data.Vehicle,
                        Registration = data.Registration,
                        Location = data.Location
                    };
                    response.Data.Add(expenseResponse);
                }
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.ErrorMessage = $"Ocurrió un error en {nameof(ExpenseService)} al obtener la información.";
                _logger.LogError(ex, $"{response.ErrorMessage} {ex.Message}");
            }
            return response;
        }

        public async Task<BaseResponseGeneric<ExpenseResponseDto>> GetAsync(int id)
        {
            var response = new BaseResponseGeneric<ExpenseResponseDto>();
            try
            {
                var data = await _expenseRepository.GetAsync(id);
                //response.Data = _mapper.Map<Expense>(data);

                var expenseResponse = new ExpenseResponseDto
                {
                    Id = id,
                    Active = data.Active,
                    CreateAt = data.CreateAt,
                    UpdatedAt = data.UpdatedAt,
                    ExpenseTypeId = data.ExpenseTypeId,
                    ExpenseDate = data.ExpenseDate,
                    Employee = data.Employee,
                    Project = data.Project,
                    Description = data.Description,
                    BudgetId = data.BudgetId,
                    File = data.File,
                    Amount = data.Amount,
                    MethodPaymentId = data.MethodPaymentId,
                    PaymentReceiptId = data.PaymentReceiptId,
                    BillNumber = data.BillNumber,
                    Vehicle = data.Vehicle,
                    Registration = data.Registration,
                    Location = data.Location
                };

                response.Data = expenseResponse;
                response.Success = true; // data != null;
            }
            catch (Exception ex)
            {
                response.ErrorMessage = $"Ocurrió un error en {nameof(ExpenseService)} al obtener la información.";
                _logger.LogError(ex, $"{response.ErrorMessage} {ex.Message}");
            }
            return response;
        }

        public async Task<BaseResponseGeneric<int>> AddAsync(Expense request)
        {
            var response = new BaseResponseGeneric<int>();
            Expense entity = new();
            try
            {
                entity = _mapper.Map<Expense>(request);
                //if (request.Image is not null)
                //{
                //    using (var memoryStream = new MemoryStream())
                //    {
                //        await request.Image.CopyToAsync(memoryStream);
                //        var content = memoryStream.ToArray();
                //        var extension = Path.GetExtension(request.Image.FileName);
                //        entity.ImageUrl = await fileStorage.SaveFile(content,
                //            extension, container, request.Image.ContentType);
                //    }
                //}
                response.Data = await _expenseRepository.AddAsync(entity); ;
                response.Success = true;
            }
            catch (Exception ex)
            {
                //await fileStorage.DeleteFile(entity.ImageUrl ?? string.Empty, container);//si ocurre algún error borrar la imagen (si existe)
                response.ErrorMessage = $"Ocurrió un error en {nameof(ExpenseService)} al añadir la información.";
                _logger.LogError(ex, "{ErrorMessage} {Message}", response.ErrorMessage, ex.Message);
            }
            return response;
        }

        public async Task<BaseResponse> UpdateAsync(int id, Expense request)
        {
            var response = new BaseResponse();
            try
            {
                var data = await _expenseRepository.GetAsync(id);
                if (data is null)
                {
                    response.ErrorMessage = $"No se ha encontrado registro con el id {id}";
                    return response;
                }

                //Actualizar valores
                data.Active = request.Active;
                data.UpdatedAt = DateTime.UtcNow;
                data.ExpenseTypeId = request.ExpenseTypeId;
                data.ExpenseDate = request.ExpenseDate;
                data.Employee = request.Employee;
                data.Project = request.Project;
                data.BudgetId = request.BudgetId;
                data.File = request.File;
                data.Amount = request.Amount;
                data.MethodPaymentId = request.MethodPaymentId;
                data.PaymentReceiptId = request.PaymentReceiptId;
                data.BillNumber = request.BillNumber;
                data.Description = request.Description;
                data.Vehicle = request.Vehicle;
                data.Registration = request.Registration;
                data.Location = request.Location;

                //_mapper.Map(request, data); //sobre escribir data nueva al objeto obtenido en la db
                //if (request.Image is not null)
                //{
                //    using (var memoryStream = new MemoryStream())
                //    {
                //        await request.Image.CopyToAsync(memoryStream);
                //        var content = memoryStream.ToArray();
                //        var extension = Path.GetExtension(request.Image.FileName);
                //        data.ImageUrl = await fileStorage.EditFile(content,
                //            extension, container, data.ImageUrl ?? string.Empty,
                //            request.Image.ContentType);
                //    }
                //}
                //else
                //{
                //    data.ImageUrl = string.Empty;
                //}

                await _expenseRepository.UpdateAsync();
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.ErrorMessage = $"Ocurrió un error en {nameof(ExpenseService)} al actualizar.";
                _logger.LogError(ex, "{ErrorMessage} {Message}", response.ErrorMessage, ex.Message);
            }
            return response;
        }

        public async Task<BaseResponse> DeleteAsync(int id)
        {
            var response = new BaseResponse();
            try
            {
                var data = await _expenseRepository.GetAsync(id);
                if (data is null)
                {
                    response.ErrorMessage = $"El registro con id {id} no fue encontrado";
                    return response;
                }

                await _expenseRepository.DeleteAsync(id);
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.ErrorMessage = $"Ocurrió un error en {nameof(ExpenseService)} al obtener la información.";
                _logger.LogError(ex, $"{response.ErrorMessage} {ex.Message}");
            }
            return response;
        }
    }
}
