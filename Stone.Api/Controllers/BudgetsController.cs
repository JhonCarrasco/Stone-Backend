using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stone.Dto.Request;
using Stone.Entities;
using Stone.Services.Interface;

namespace Stone.Api.Controllers
{
    [ApiController]
    [Route("api/budgets")]
    public class BudgetsController : ControllerBase
    {
        private readonly IBudgetService budgetService;
        private readonly ILogger<BudgetsController> logger;

        public BudgetsController(IBudgetService budgetService, ILogger<BudgetsController> logger)
        {
            this.budgetService = budgetService;
            this.logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var response = await budgetService.GetAsync();
            return response.Success ? Ok(response) : BadRequest(response);
        }
        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            var response = await budgetService.GetAsync(id);
            return response.Success ? Ok(response) : NotFound(response);
        }
        [HttpPost]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = Constants.RoleAdmin)]
        public async Task<IActionResult> Post(BudgetRequestDto budgetRequestDTO)
        {
            var response = await budgetService.AddAsync(budgetRequestDTO);
            return response.Success ? Ok(response) : BadRequest(response);
        }

        [HttpPut("{id:int}")]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = Constants.RoleAdmin)]
        public async Task<IActionResult> Put(int id, BudgetRequestDto budgetRequestDTO)
        {
            var response = await budgetService.UpdateAsync(id, budgetRequestDTO);
            return response.Success ? Ok(response) : BadRequest(response);
        }

        [HttpDelete("{id:int}")]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = Constants.RoleAdmin)]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var response = await budgetService.DeleteAsync(id);
            return response.Success ? Ok(response) : BadRequest(response);
        }
    }
}
