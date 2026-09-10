using Google.Apis.Download;
using Google.Apis.Drive.v3;
using Google.Apis.Drive.v3.Data;
using Google.Apis.Upload;
using Microsoft.AspNetCore.Http;
using Stone.Dto.Response;
using Stone.Services.Interface;

namespace Stone.Services.Implementation
{
    public class GoogleDriveService : IGoogleDriveService
    {
        private readonly DriveService _driveService;

        public GoogleDriveService(DriveService driveService)
        {
            this._driveService = driveService;
        }

        public async Task<BaseResponseGeneric<List<FileResponse>>> GetFiles(string folderId)
        {
            BaseResponseGeneric<List<FileResponse>> response = new BaseResponseGeneric<List<FileResponse>>();
            
            var filesList = new List<Google.Apis.Drive.v3.Data.File>();
            string? nextPageToken = null;

            do
            {
                // 1. Initialize the list request
                FilesResource.ListRequest listRequest = _driveService.Files.List();

                // 2. Filter by the target parent folder ID and exclude deleted files
                listRequest.Q = $"'{folderId}' in parents and trashed = false";

                // 3. Define the specific fields you want returned (saves bandwidth)
                listRequest.Fields = "nextPageToken, files(id, name, mimeType, createdTime)";
                listRequest.PageSize = 100;
                listRequest.PageToken = nextPageToken;

                // 4. Handle Shared Drives if applicable
                listRequest.SupportsAllDrives = true;
                listRequest.IncludeItemsFromAllDrives = true;

                // 5. Execute the request
                FileList result = await listRequest.ExecuteAsync();

                if (result.Files != null)
                {
                    filesList.AddRange(result.Files);
                }

                // Paginate if there are more than 100 files
                nextPageToken = result.NextPageToken;

            } while (!string.IsNullOrEmpty(nextPageToken));


            response.Success = true;
            response.Data = filesList
                .Select(f => new FileResponse
                {                    
                    Id = f.Id,
                    Name = f.Name,
                })
                .ToList();
            return response;
        }

        public async Task<BaseResponseGeneric<string>> CreateFolder(string parentId, string folderName)
        {
            BaseResponseGeneric<string> response = new BaseResponseGeneric<string>();
            try
            {
                // File metadata
                var fileMetadata = new Google.Apis.Drive.v3.Data.File()
                {
                    Name = folderName,
                    MimeType = "application/vnd.google-apps.folder",
                    Parents = new List<string> { parentId }
                };

                // Create a new folder on drive.
                var request = _driveService.Files.Create(fileMetadata);
                request.Fields = "id";
                request.SupportsAllDrives = true;
                var file = await request.ExecuteAsync();
                // Prints the created folder id.
                Console.WriteLine("Folder ID: " + file.Id);
                response.Success = true;
                response.Data = file.Id;
                
            }
            catch (Exception e)
            {
                // TODO(developer) - handle error appropriately
                if (e is AggregateException)
                {
                    Console.WriteLine("Credential Not found");
                    response.Success = false;
                    response.ErrorMessage = $"Upload failed: {e.Message}";
                }
                else
                {
                    throw;
                }
            }
            return response;
        }

        public async Task<BaseResponseGeneric<string>> UploadToFolder(IFormFile file, string folderId)
        {
            BaseResponseGeneric<string> response = new BaseResponseGeneric<string>();
            try
            {
                if (file == null || file.Length == 0)
                {
                    response.Success = false;
                    response.ErrorMessage = "File is null or empty.";
                    return response;
                }

                var driveFile = new Google.Apis.Drive.v3.Data.File()
                {
                    Name = file.FileName,
                    Parents = new List<string>() { folderId }
                };

                using var stream = file.OpenReadStream();

                var request = _driveService.Files.Create(
                    driveFile,
                    stream,
                    file.ContentType);

                request.SupportsAllDrives = true;
                request.Fields = "id";

                request.ProgressChanged += ProgressChanged;
                request.ResponseReceived += ResponseReceived;

                var result = await request.UploadAsync();

                if (result.Status == UploadStatus.Failed)
                {
                    response.Success = false;
                    response.ErrorMessage = $"Upload failed: {result.Exception.Message}";
                    return response;
                }

                response.Success = true;
                response.Data = request.ResponseBody.Id;
            }
            catch (Exception e)
            {
                // TODO(developer) - handle error appropriately
                if (e is AggregateException)
                {
                    response.ErrorMessage = "Credential Not found";
                }
                else if (e is FileNotFoundException)
                {
                    response.ErrorMessage = "File not found";
                }
                else if (e is DirectoryNotFoundException)
                {
                    response.ErrorMessage = "Directory Not found";
                }
                else
                {
                    throw;
                }
            }
                        
            return response;
        }

        public async Task<BaseResponseGeneric<string>> DownloadFile(string fileId)
        {
            BaseResponseGeneric<string> response = new BaseResponseGeneric<string>();
            try
            {
                var request = _driveService.Files.Get(fileId);
                var stream = new MemoryStream();

                // Add a handler which will be notified on progress changes.
                // It will notify on each chunk download and when the
                // download is completed or failed.
                request.MediaDownloader.ProgressChanged +=
                    progress =>
                    {
                        switch (progress.Status)
                        {
                            case DownloadStatus.Downloading:
                                {
                                    Console.WriteLine(progress.BytesDownloaded);
                                    break;
                                }
                            case DownloadStatus.Completed:
                                {
                                    Console.WriteLine("Download complete.");
                                    response.Success = true;
                                    break;
                                }
                            case DownloadStatus.Failed:
                                {
                                    Console.WriteLine("Download failed.");
                                    response.Success = false;
                                    response.ErrorMessage = "Download failed.";
                                    break;
                                }
                        }
                    };
                await request.DownloadAsync(stream);
                // Convert byte array from the stream to a Base64 string
                byte[] fileBytes = stream.ToArray();
                var base64String = Convert.ToBase64String(fileBytes);
                response.Data = base64String;
            }
            catch (Exception e)
            {
                // TODO(developer) - handle error appropriately
                if (e is AggregateException)
                {
                    Console.WriteLine("Credential Not found");
                    response.Success = false;
                    response.ErrorMessage = e.Message;
                }
                else
                {
                    throw;
                }
            }
            
            return response;
        }

        private void ProgressChanged(IUploadProgress progress)
        {
            Console.WriteLine($"Status: {progress.Status}, Bytes Sent: {progress.BytesSent}");
        }

        private void ResponseReceived(Google.Apis.Drive.v3.Data.File file)
        {
            Console.WriteLine($"File ID {file.Id} uploaded.");
        }


    }
}
