using AutoMapper;
using Microsoft.Extensions.Logging;
using Stone.Api.Enums;
using Stone.Dto.Request;
using Stone.Dto.Response;
using Stone.Entities;
using Stone.Repositories.Interface;
using Stone.Services.Interface;

namespace Stone.Services.Implementation
{
    public class ExpenseService(
        IExpenseRepository expenseRepository,
        IFileDriveRepository fileDriveRepository,
        ILogger<IMaterialService> logger,
        IMapper mapper) : IExpenseService
    {
        private readonly IExpenseRepository _expenseRepository = expenseRepository;
        private readonly IFileDriveRepository _fileDriveRepository = fileDriveRepository;
        private readonly ILogger<IMaterialService> _logger = logger;
        private readonly IMapper _mapper = mapper;

        public async Task<BaseResponseGeneric<ICollection<ExpenseResponseDto>>> GetAsync(string searchText, PaginationDto pagination)
        {
            var response = new BaseResponseGeneric<ICollection<ExpenseResponseDto>>();
            try
            {
                var EntityList = await _expenseRepository.GetAsync(predicate: s =>
                    s.Amount.ToString()!.Contains(searchText ?? string.Empty)
                    || (s.Project != null && s.Project.Contains(searchText ?? string.Empty))
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
                        Employee = data.Employee ?? string.Empty,
                        Project = data.Project,
                        Description = data.Description,
                        BudgetId = data.BudgetId,
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
                if (data == null)
                {
                    response.ErrorMessage = $"No se ha encontrado registro con el id {id}";
                    return response;
                }

                var filesEntity = await _fileDriveRepository.GetAsync(
                    predicate: s => s.DocumentId == data.Id
                        && s.DocumentType == (int)DocumentType.ExpenseReceipt,
                    orderBy: x => x.Id);

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
                    Amount = data.Amount,
                    MethodPaymentId = data.MethodPaymentId,
                    PaymentReceiptId = data.PaymentReceiptId,
                    BillNumber = data.BillNumber,
                    Vehicle = data.Vehicle,
                    Registration = data.Registration,
                    Location = data.Location,
                    Files = filesEntity
                };

                response.Data = expenseResponse;
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.ErrorMessage = $"Ocurrió un error en {nameof(ExpenseService)} al obtener la información.";
                _logger.LogError(ex, $"{response.ErrorMessage} {ex.Message}");
            }
            return response;
        }

        public async Task<BaseResponseGeneric<int>> AddAsync(ExpenseRequestDto request)
        {
            var response = new BaseResponseGeneric<int>();
            Expense entity = new();
            try
            {
                var newExpense = new Expense
                {
                    ExpenseTypeId = request.ExpenseTypeId,
                    ExpenseDate = request.ExpenseDate,
                    Employee = request.Employee,
                    Project = request.Project,
                    BudgetId = request.BudgetId > 0 ? request.BudgetId : null,
                    Amount = request.Amount,
                    MethodPaymentId = request.MethodPaymentId,
                    PaymentReceiptId = request.PaymentReceiptId,
                    BillNumber = request.BillNumber,
                    Description = request.Description,
                    Vehicle = request.Vehicle,
                    Registration = request.Registration,
                    Location = request.Location
                };

                response.Data = await _expenseRepository.AddAsync(newExpense); ;
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.ErrorMessage = $"Ocurrió un error en {nameof(ExpenseService)} al añadir la información.";
                _logger.LogError(ex, "{ErrorMessage} {Message}", response.ErrorMessage, ex.Message);
            }
            return response;
        }

        public async Task<BaseResponse> UpdateAsync(int id, ExpenseRequestDto request)
        {
            var response = new BaseResponseGeneric<int>();
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
                data.Amount = request.Amount;
                data.MethodPaymentId = request.MethodPaymentId;
                data.PaymentReceiptId = request.PaymentReceiptId;
                data.BillNumber = request.BillNumber;
                data.Description = request.Description;
                data.Vehicle = request.Vehicle;
                data.Registration = request.Registration;
                data.Location = request.Location;

                await _expenseRepository.UpdateAsync();

                response.Data = id;
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
