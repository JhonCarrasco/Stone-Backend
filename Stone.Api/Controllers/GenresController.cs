using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stone.Dto.Request;
using Stone.Entities.Generic;
using Stone.Services.Interface;

namespace Stone.Api.Controllers
{
    [ApiController]
    [Route("api/genres")]
    public class GenresController : ControllerBase
    {
        private readonly IGenreService genreService;
        private readonly ILogger<GenresController> logger;

        public GenresController(IGenreService genreService, ILogger<GenresController> logger)
        {
            this.genreService = genreService;
            this.logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var response = await genreService.GetAsync();
            return response.Success ? Ok(response) : BadRequest(response);
        }
        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            var response = await genreService.GetAsync(id);
            return response.Success ? Ok(response) : NotFound(response);
        }
        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = Constants.RoleAdmin)]
        public async Task<IActionResult> Post(GenreRequestDto genreRequestDTO)
        {
            var response = await genreService.AddAsync(genreRequestDTO);
            return response.Success ? Ok(response) : BadRequest(response);
        }

        [HttpPut("{id:int}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = Constants.RoleAdmin)]
        public async Task<IActionResult> Put(int id, GenreRequestDto genreRequestDTO)
        {
            var response = await genreService.UpdateAsync(id, genreRequestDTO);
            return response.Success ? Ok(response) : BadRequest(response);
        }

        [HttpDelete("{id:int}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = Constants.RoleAdmin)]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var response = await genreService.DeleteAsync(id);
            return response.Success ? Ok(response) : BadRequest(response);
        }
    }
}
