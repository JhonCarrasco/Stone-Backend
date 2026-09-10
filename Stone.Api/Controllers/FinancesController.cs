using Microsoft.AspNetCore.Mvc;
using Stone.Dto.Request;
using Stone.Entities;
using Stone.Services.Interface;

namespace Stone.Api.Controllers
{
    [Route("api/finances")]
    [ApiController]
    public class FinancesController : ControllerBase
    {
        private readonly IExpenseService _expenseService;
        private readonly ILogger<FinancesController> _logger;

        public FinancesController(
            IExpenseService expenseService,
            ILogger<FinancesController> logger)
        {
            this._expenseService = expenseService;
            this._logger = logger;
        }

        [HttpGet("expenses")]
        public async Task<IActionResult> Get([FromQuery] string? searchText, [FromQuery] PaginationDto pagination)
        {
            var response = await _expenseService.GetAsync(searchText, pagination);
            return response.Success ? Ok(response) : BadRequest(response);
        }
        [HttpGet("expenses/{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            var response = await _expenseService.GetAsync(id);
            return response.Success ? Ok(response) : NotFound(response);
        }
        [HttpPost("expenses")]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = Constants.RoleAdmin)]
        public async Task<IActionResult> Post(Expense request)
        {
            var response = await _expenseService.AddAsync(request);
            return response.Success ? Ok(response) : BadRequest(response);
        }

        [HttpPut("expenses/{id:int}")]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = Constants.RoleAdmin)]
        public async Task<IActionResult> Put(int id, Expense request)
        {
            var response = await _expenseService.UpdateAsync(id, request);
            return response.Success ? Ok(response) : BadRequest(response);
        }

        [HttpDelete("expenses/{id:int}")]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = Constants.RoleAdmin)]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var response = await _expenseService.DeleteAsync(id);
            return response.Success ? Ok(response) : BadRequest(response);
        }
    }
}
