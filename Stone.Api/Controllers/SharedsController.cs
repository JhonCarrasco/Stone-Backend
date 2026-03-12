using Microsoft.AspNetCore.Mvc;
using Stone.Services.Interface;

namespace Stone.Api.Controllers
{
    [Route("api/shareds")]
    [ApiController]
    public class SharedsController : ControllerBase
    {
        private readonly ISharedService sharedService;
        private readonly ILogger<SharedsController> logger;

        public SharedsController(ISharedService sharedService, ILogger<SharedsController> logger)
        {
            this.sharedService = sharedService;
            this.logger = logger;
        }

        [HttpGet("Banks")]
        public async Task<IActionResult> GetBank()
        {
            var response = await sharedService.GetBankAsync();
            return response.Success ? Ok(response) : BadRequest(response);
        }

        [HttpGet("Categories")]
        public async Task<IActionResult> GetCategory()
        {
            var response = await sharedService.GetCategoryAsync();
            return response.Success ? Ok(response) : BadRequest(response);
        }

        [HttpGet("Communes")]
        public async Task<IActionResult> GetCommune()
        {
            var response = await sharedService.GetCommuneAsync();
            return response.Success ? Ok(response) : BadRequest(response);
        }

        [HttpGet("Manufacturers")]
        public async Task<IActionResult> GetManufacturer()
        {
            var response = await sharedService.GetManufacturerAsync();
            return response.Success ? Ok(response) : BadRequest(response);
        }

        [HttpGet("Regions")]
        public async Task<IActionResult> GetRegion()
        {
            var response = await sharedService.GetRegionAsync();
            return response.Success ? Ok(response) : BadRequest(response);
        }

        [HttpGet("TypeAccounts")]
        public async Task<IActionResult> GetTypeAccount()
        {
            var response = await sharedService.GetTypeAccountAsync();
            return response.Success ? Ok(response) : BadRequest(response);
        }

        [HttpGet("UnitMeasurements")]
        public async Task<IActionResult> GetUnitMeasurement()
        {
            var response = await sharedService.GetUnitMeasurementAsync();
            return response.Success ? Ok(response) : BadRequest(response);
        }
    }
}
