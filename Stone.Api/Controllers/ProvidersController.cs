using Microsoft.AspNetCore.Mvc;
using Stone.Dto.Request;
using Stone.Entities;
using Stone.Services.Interface;

namespace Stone.Api.Controllers
{
    [Route("api/providers")]
    [ApiController]
    public class ProvidersController : ControllerBase
    {
        private readonly IProviderService providerService;
        private readonly ILogger<ProvidersController> logger;

        public ProvidersController(IProviderService providerService, ILogger<ProvidersController> logger)
        {
            this.providerService = providerService;
            this.logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] string? searchText, [FromQuery] PaginationDto pagination)
        {
            var response = await providerService.GetAsync(searchText, pagination);
            return response.Success ? Ok(response) : BadRequest(response);
        }
        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            var response = await providerService.GetAsync(id);
            return response.Success ? Ok(response) : NotFound(response);
        }
        [HttpPost]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = Constants.RoleAdmin)]
        public async Task<IActionResult> Post(ProviderRequestDto request)
        {
            var response = await providerService.AddAsync(request);
            return response.Success ? Ok(response) : BadRequest(response);
        }

        [HttpPut("{id:int}")]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = Constants.RoleAdmin)]
        public async Task<IActionResult> Put(int id, ProviderRequestDto request)
        {
            var response = await providerService.UpdateAsync(id, request);
            return response.Success ? Ok(response) : BadRequest(response);
        }

        [HttpDelete("{id:int}")]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = Constants.RoleAdmin)]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var response = await providerService.DeleteAsync(id);
            return response.Success ? Ok(response) : BadRequest(response);
        }
    }
}
