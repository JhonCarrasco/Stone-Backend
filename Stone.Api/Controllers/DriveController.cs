using Microsoft.AspNetCore.Mvc;
using Stone.Services.Interface;

namespace Stone.Api.Controllers
{
    [Route("api/drive")]
    [ApiController]
    public class DriveController : ControllerBase
    {
        private readonly IGoogleDriveService _googleDriveService;

        //public const string DirectoryId = "0ADX9ZM8JD_tiUk9PVA";

        public DriveController(IGoogleDriveService googleDriveService)
        {
            this._googleDriveService = googleDriveService;
        }

        [HttpGet("files")]
        public async Task<IActionResult> GetFiles(string folderId)
        {
            var response = await _googleDriveService.GetFiles(folderId);
            return response.Success ? Ok(response) : BadRequest(response);
        }

        [HttpPost("create-folder")]
        public async Task<IActionResult> CreateFolder(string parentId, string folderName)
        {
            //try
            //{
            //    // File metadata
            //    var fileMetadata = new Google.Apis.Drive.v3.Data.File()
            //    {
            //        Name = "Invoices2",
            //        MimeType = "application/vnd.google-apps.folder",
            //        Parents = new List<string> { folderId }
            //    };

            //    // Create a new folder on drive.
            //    var request = _driveService.Files.Create(fileMetadata);
            //    request.Fields = "id";
            //    request.SupportsAllDrives = true;
            //    var file = request.Execute();
            //    // Prints the created folder id.
            //    Console.WriteLine("Folder ID: " + file.Id);
            //    return Ok(file);
            //}
            //catch (Exception e)
            //{
            //    // TODO(developer) - handle error appropriately
            //    if (e is AggregateException)
            //    {
            //        Console.WriteLine("Credential Not found");
            //        return StatusCode(500, "Credential Not found");
            //    }
            //    else
            //    {
            //        throw;
            //    }
            //}

            var response = await _googleDriveService.CreateFolder(parentId, folderName);
            return response.Success ? Ok(response) : BadRequest(response);

        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadToDrive(IFormFile file, string folderId)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file selected.");

            //var driveFile = new Google.Apis.Drive.v3.Data.File()
            //{
            //    Name = file.FileName,
            //    Parents = new List<string>() { folderId }
            //};

            //using var stream = file.OpenReadStream();

            //var request = _driveService.Files.Create(
            //    driveFile,
            //    stream,
            //    file.ContentType);

            //request.SupportsAllDrives = true;

            //request.ProgressChanged += ProgressChanged;
            //request.ResponseReceived += ResponseReceived;

            //var result = await request.UploadAsync();

            //if (result.Status == UploadStatus.Failed)
            //    return StatusCode(500, $"Upload failed: {result.Exception.Message}");

            //return Ok(new { message = "Uploaded successfully!" });

            var response = await _googleDriveService.UploadToFolder(file, folderId);
            return response.Success ? Ok(response) : BadRequest(response);
        }

        [HttpGet("download")]
        public async Task<IActionResult> DownloadFile(string fileId)
        {
            var response = await _googleDriveService.DownloadFile(fileId);
            return response.Success ? Ok(response) : BadRequest(response);
        }
    }
}
