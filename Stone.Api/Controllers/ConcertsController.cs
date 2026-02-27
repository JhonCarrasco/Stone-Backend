using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stone.Dto.Request;
using Stone.Entities.Generic;
using Stone.Services.Interface;

namespace Stone.Api.Controllers
{
    [ApiController]
    [Route("api/concerts")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = Constants.RoleAdmin)]
    public class ConcertsController : ControllerBase
    {
        private readonly IConcertService concertService;
        private readonly ILogger<ConcertsController> logger;

        public ConcertsController(
            IConcertService concertService,
            ILogger<ConcertsController> logger)
        {
            this.concertService = concertService;
            this.logger = logger;
        }

        [HttpGet("title")]
        [AllowAnonymous]
        public async Task<IActionResult> Get(string? title, [FromQuery]PaginationDto pagination)
        {
            var concerts = await concertService.GetAsync(title, pagination);
            return Ok(concerts);
        }

        [HttpGet("{id:int}")]
        [AllowAnonymous]
        public async Task<IActionResult> Get(int id)
        {
            var response = await concertService.GetAsync(id);
            return response.Success ? Ok(response) : NotFound(response);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromForm]ConcertRequestDto request)
        {
            var response = await concertService.AddAsync(request);
            return response.Success ? Ok(response) : BadRequest(response);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Put(int id, [FromForm]ConcertRequestDto request)
        {
            var response = await concertService.UpdateAsync(id, request);
            return response.Success ? Ok(response) : BadRequest(response);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await concertService.DeleteAsync(id);
            return response.Success ? Ok(response) : BadRequest(response);
        }

        [HttpPatch("{id:int}")]
        public async Task<IActionResult> Patch(int id)
        {
            return Ok(await concertService.FinalizeAsync(id));
        }
    }
}
