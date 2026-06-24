using Microsoft.AspNetCore.Mvc;
using Stone.Dto.Request;
using Stone.Services.Implementation;
using Stone.Services.Interface;

namespace Stone.Api.Controllers
{
    [Route("api/materials")]
    [ApiController]
    public class MaterialsController : ControllerBase
    {
        private readonly IReceptionGuideService _receptionGuideService;
        private readonly IDispatchGuideService _dispatchGuideService;
        private readonly IVoucherService _voucherService;
        private readonly ILogger<MaterialsController> _logger;

        public MaterialsController(IReceptionGuideService receptionGuideService,
            IDispatchGuideService dispatchGuideService,
            IVoucherService voucherService,
            ILogger<MaterialsController> logger)
        {
            this._receptionGuideService = receptionGuideService;
            this._dispatchGuideService = dispatchGuideService;
            this._voucherService = voucherService;
            this._logger = logger;
        }

        [HttpGet("receptions")]
        public async Task<IActionResult> GetReceptions([FromQuery] string? searchText, [FromQuery] PaginationDto pagination)
        {
            var response = await _receptionGuideService.GetAsync(searchText, pagination);
            return response.Success ? Ok(response) : BadRequest(response);
        }

        [HttpPost("receptions")]
        public async Task<IActionResult> PostReceptions(MaterialGenericRequestDto request)
        {
            var response = await _receptionGuideService.AddAsync(request);
            return response.Success ? Ok(response) : BadRequest(response);
        }

        [HttpPut("receptions/{id:int}")]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = Constants.RoleAdmin)]
        public async Task<IActionResult> PutReceptions(int id, MaterialGenericRequestDto request)
        {
            var response = await _receptionGuideService.UpdateAsync(id, request);
            return response.Success ? Ok(response) : BadRequest(response);
        }

        [HttpGet("receptions/{id:int}")]
        public async Task<IActionResult> GetReceptions(int id)
        {
            var response = await _receptionGuideService.GetAsync(id);
            return response.Success ? Ok(response) : NotFound(response);
        }

        [HttpDelete("receptions/{id:int}")]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = Constants.RoleAdmin)]
        public async Task<IActionResult> DeleteReceptions(int id)
        {
            var response = await _receptionGuideService.DeleteAsync(id);
            return response.Success ? Ok(response) : BadRequest(response);
        }

        [HttpGet("dispatches")]
        public async Task<IActionResult> GetDispatches([FromQuery] string? searchText, [FromQuery] PaginationDto pagination)
        {
            var response = await _dispatchGuideService.GetAsync(searchText, pagination);
            return response.Success ? Ok(response) : BadRequest(response);
        }

        [HttpPost("dispatches")]
        public async Task<IActionResult> PostDispatches(MaterialGenericRequestDto request)
        {
            var response = await _dispatchGuideService.AddAsync(request);
            return response.Success ? Ok(response) : BadRequest(response);
        }

        [HttpPut("dispatches/{id:int}")]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = Constants.RoleAdmin)]
        public async Task<IActionResult> PutDispatches(int id, MaterialGenericRequestDto request)
        {
            var response = await _dispatchGuideService.UpdateAsync(id, request);
            return response.Success ? Ok(response) : BadRequest(response);
        }

        [HttpGet("dispatches/{id:int}")]
        public async Task<IActionResult> GetDispatches(int id)
        {
            var response = await _dispatchGuideService.GetAsync(id);
            return response.Success ? Ok(response) : NotFound(response);
        }

        [HttpDelete("dispatches/{id:int}")]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = Constants.RoleAdmin)]
        public async Task<IActionResult> DeleteDispatches(int id)
        {
            var response = await _dispatchGuideService.DeleteAsync(id);
            return response.Success ? Ok(response) : BadRequest(response);
        }

        [HttpGet("vouchers")]
        public async Task<IActionResult> GetVouchers([FromQuery] string? searchText, [FromQuery] PaginationDto pagination)
        {
            var response = await _voucherService.GetAsync(searchText, pagination);
            return response.Success ? Ok(response) : BadRequest(response);
        }

        [HttpPost("vouchers")]
        public async Task<IActionResult> PostVouchers(MaterialGenericRequestDto request)
        {
            var response = await _voucherService.AddAsync(request);
            return response.Success ? Ok(response) : BadRequest(response);
        }

        [HttpPut("vouchers/{id:int}")]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = Constants.RoleAdmin)]
        public async Task<IActionResult> PutVouchers(int id, MaterialGenericRequestDto request)
        {
            var response = await _voucherService.UpdateAsync(id, request);
            return response.Success ? Ok(response) : BadRequest(response);
        }

        [HttpGet("vouchers/{id:int}")]
        public async Task<IActionResult> GetVouchers(int id)
        {
            var response = await _voucherService.GetAsync(id);
            return response.Success ? Ok(response) : NotFound(response);
        }

        [HttpDelete("vouchers/{id:int}")]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = Constants.RoleAdmin)]
        public async Task<IActionResult> DeleteVouchers(int id)
        {
            var response = await _voucherService.DeleteAsync(id);
            return response.Success ? Ok(response) : BadRequest(response);
        }
    }
}
