using Microsoft.AspNetCore.Mvc;
using Stone.Dto.Request;
using Stone.Services.Interface;

namespace Stone.Api.Controllers
{
    [Route("api/customers")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerService customerService;
        private readonly ILogger<CustomersController> logger;

        public CustomersController(ICustomerService customerService, ILogger<CustomersController> logger)
        {
            this.customerService = customerService;
            this.logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var response = await customerService.GetAsync();
            return response.Success ? Ok(response) : BadRequest(response);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            var response = await customerService.GetAsync(id);
            return response.Success ? Ok(response) : NotFound(response);
        }

        [HttpPost]
        public async Task<IActionResult> Post(CustomerRequestDto customerRequestDTO)
        {
            var response = await customerService.AddAsync(customerRequestDTO);
            return response.Success ? Ok(response) : BadRequest(response);
        }

        [HttpPut("{id:int}")]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = Constants.RoleAdmin)]
        public async Task<IActionResult> Put(int id, CustomerRequestDto customerRequestDTO)
        {
            var response = await customerService.UpdateAsync(id, customerRequestDTO);
            return response.Success ? Ok(response) : BadRequest(response);
        }

        [HttpDelete("{id:int}")]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = Constants.RoleAdmin)]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var response = await customerService.DeleteAsync(id);
            return response.Success ? Ok(response) : BadRequest(response);
        }

    }
}
