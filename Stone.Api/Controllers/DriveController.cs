using Microsoft.AspNetCore.Mvc;
using Stone.Dto.Request;
using Stone.Services.Interface;
using System.Net.Mime;

namespace Stone.Api.Controllers
{
    [Route("api/drive")]
    [ApiController]
    public class DriveController : ControllerBase
    {
        private readonly IGoogleDriveService _googleDriveService;


        public DriveController(IGoogleDriveService googleDriveService)
        {
            this._googleDriveService = googleDriveService;
        }

        [HttpGet("files/{folderId}")]
        public async Task<IActionResult> GetFiles(string folderId)
        {
            var response = await _googleDriveService.GetFilesByFolderId(folderId);
            return response.Success ? Ok(response) : BadRequest(response);
        }

        [HttpPost("create-folder")]
        public async Task<IActionResult> CreateFolder(string parentId, string folderName)
        {
            var response = await _googleDriveService.CreateFolder(parentId, folderName);
            return response.Success ? Ok(response) : BadRequest(response);

        }

        [HttpPost("upload")]
        [Consumes("multipart/form-data")]
        [Produces(MediaTypeNames.Application.Json)]
        public async Task<IActionResult> UploadToDrive([FromForm] FileRequestDto request)
        {
            if (request.Files == null || request.Files.Count == 0)
                return BadRequest("No files selected.");

            var response = await _googleDriveService.UploadToFolder(request);
            return response.Success ? Ok(response) : BadRequest(response);
        }

        [HttpGet("download")]
        public async Task<IActionResult> DownloadFile(string fileId)
        {
            var response = await _googleDriveService.DownloadFile(fileId);
            return response.Success ? Ok(response) : BadRequest(response);
        }

        [HttpDelete("{fileId}")]
        public async Task<IActionResult> DeleteFile(string fileId)
        {
            var response = await _googleDriveService.DeleteFileAsync(fileId);
            return response.Success ? Ok(response) : BadRequest(response);
        }
    }
}
