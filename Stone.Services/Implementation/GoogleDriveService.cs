using Google;
using Google.Apis.Download;
using Google.Apis.Drive.v3;
using Google.Apis.Drive.v3.Data;
using Google.Apis.Upload;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Stone.Dto.Request;
using Stone.Dto.Response;
using Stone.Entities;
using Stone.Repositories.Interface;
using Stone.Services.Interface;

namespace Stone.Services.Implementation
{
    public class GoogleDriveService : IGoogleDriveService
    {
        private readonly DriveService _driveService;
        private readonly IFileDriveRepository _fileDriveRepository;
        private readonly IOptions<AppSettings> _options;

        public GoogleDriveService(DriveService driveService, IFileDriveRepository fileDriveRepository, IOptions<AppSettings> options)
        {
            this._driveService = driveService;
            this._fileDriveRepository = fileDriveRepository;
            this._options = options;
        }

        public async Task<BaseResponseGeneric<List<FileResponseDto>>> GetFilesByFolderId(string folderId)
        {
            BaseResponseGeneric<List<FileResponseDto>> response = new BaseResponseGeneric<List<FileResponseDto>>();
            
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
                .Select(f => new FileResponseDto
                {                    
                    Id = f.Id,
                    Name = f.Name,
                })
                .ToList();
            return response;
        }

        //public async Task<string> GetFolder(string folderName)
        //{
        //    var listRequest = _driveService.Files.List();
        //    // Filter by folder mimeType, exact name match, and exclude trashed items
        //    listRequest.Q = $"mimeType = 'application/vnd.google-apps.folder' and name = '{folderName}' and trashed = false";
        //    listRequest.Fields = "files(id, name)";
        //    listRequest.PageSize = 1;

        //    var result = await listRequest.ExecuteAsync();
        //    return result.Files.FirstOrDefault()?.Id;
        //}

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
                // handle error appropriately
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

        public async Task<BaseResponseGeneric<ICollection<FileDrive>>> UploadToFolder(FileRequestDto request)
        {
            BaseResponseGeneric<ICollection<FileDrive>> response = new BaseResponseGeneric<ICollection<FileDrive>>();
            try
            {
                var createFolderResponse = new BaseResponseGeneric<string>();

                if (request.Files == null || request.Files.Count == 0)
                {
                    response.Success = false;
                    response.ErrorMessage = "File is null or empty.";
                    return response;
                }

                // Validate folderId exists in Google Drive before proceeding with upload
                var folderExists = await FolderExists(request.FolderId!);
                
                if (!folderExists)
                {
                    var (FolderTypeRootId, folderName) = GetFolderTypeRoot(request.DocumentType);
                    string fullFolderName = folderName + (string.IsNullOrEmpty(request.Rut) ? "" : $"_{request.Rut.Replace("-", "")}") + $"-{request.DocumentId}";
                    // Create folder if it doesn't exist
                    createFolderResponse = await CreateFolder(FolderTypeRootId, fullFolderName); 
                }

                // Ensure response.Data is initialized to avoid null dereference
                response.Data = new List<FileDrive>();

                // Fix: declare these variables before use
                var documentId = request.DocumentId;
                var documentType = request.DocumentType;

                foreach (var file in request.Files)
                {
                    var parentFolderId = request.FolderId.IsNullOrEmpty() ? createFolderResponse.Data : request.FolderId;
                    if (string.IsNullOrEmpty(parentFolderId))
                    {
                        response.Success = false;
                        response.ErrorMessage = "Parent folder ID is null or empty.";
                        return response;
                    }


                    var driveFile = new Google.Apis.Drive.v3.Data.File()
                    {
                        Name = file.FileName,
                        Parents = new List<string>() { parentFolderId }
                    };

                    using var stream = file.OpenReadStream();

                    var uploadRequest = _driveService.Files.Create(
                        driveFile,
                        stream,
                        file.ContentType);

                    uploadRequest.SupportsAllDrives = true;
                    uploadRequest.Fields = "id, name, size, mimeType, md5Checksum, modifiedTime";

                    uploadRequest.ProgressChanged += ProgressChanged;
                    uploadRequest.ResponseReceived += ResponseReceived;

                    var result = await uploadRequest.UploadAsync();

                    if (result.Status == UploadStatus.Failed)
                    {
                        response.Success = false;
                        response.ErrorMessage = $"Upload failed: {result.Exception.Message}";
                        return response;
                    }

                    var fileDriveEntity = new FileDrive
                    {
                        FileId = uploadRequest.ResponseBody.Id,
                        FileName = driveFile.Name,
                        FileExtention = Path.GetExtension(file.FileName),
                        ContentType = uploadRequest.ContentType,
                        FolderParentId = parentFolderId,
                        DocumentId = documentId,
                        DocumentType = documentType,
                        Size = file.Length,
                    };

                    var fileDriveIdResponse = await _fileDriveRepository.AddAsync(fileDriveEntity);

                    if(fileDriveIdResponse > 0)
                    {
                        fileDriveEntity.Id = fileDriveIdResponse;
                        response.Data.Add(fileDriveEntity);
                    }                    
                }
                response.Success = true;
            }
            catch (Exception e)
            {
                // handle error appropriately
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

        /// <summary>
        /// Permanently deletes a file by its ID.
        /// </summary>
        /// <param name="fileId">The ID of the file to delete.</param>
        /// <returns>True if deletion succeeded, false otherwise.</returns>
        public async Task<BaseResponseGeneric<bool>> DeleteFileAsync(string fileId)
        {
            if (string.IsNullOrWhiteSpace(fileId))
                throw new ArgumentException("File ID cannot be null or empty.", nameof(fileId));

            try
            {
                var deleteRequest = _driveService.Files.Delete(fileId);
                deleteRequest.SupportsAllDrives = true; // Required if file is on a Shared Drive

                var task1 = deleteRequest.ExecuteAsync();
                var task2 = _fileDriveRepository.DeleteByFileIdAsync(fileId);
                await Task.WhenAll(task1, task2);

                return new BaseResponseGeneric<bool> { Success = true, Data = true };
            }
            catch (GoogleApiException ex) when (ex.HttpStatusCode == System.Net.HttpStatusCode.NotFound)
            {
                // File does not exist
                return new BaseResponseGeneric<bool> { Success = false, Data = false, ErrorMessage = "File not found." };
            }
        }
          
        private async Task<bool> FolderExists(string folderId)
        {
            try
            {
                if (folderId == null || folderId.IsNullOrEmpty())
                {
                    return false;
                }

                var request = _driveService.Files.Get(folderId);
                request.Fields = "id, mimeType";
                request.SupportsAllDrives = true;
                var file = await request.ExecuteAsync();
                bool exists = file != null && file.MimeType == "application/vnd.google-apps.folder";
                return exists;
            }
            catch (Google.GoogleApiException ex) when (ex.HttpStatusCode == System.Net.HttpStatusCode.NotFound)
            {
                // Folder does not exist
                return false;
            }
        }
                
        public async Task<(bool, string?)> FileExistsByNameAsync(string fileName, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(fileName))
            {
                throw new ArgumentException("Filename cannot be null or empty.", nameof(fileName));
            }

            // Escape single quotes and backslashes to prevent query syntax errors
            string sanitizedName = fileName.Replace("\\", "\\\\").Replace("'", "\\'");

            // Search by exact name and ensure the file is not in trash
            var listRequest = _driveService.Files.List();
            listRequest.Q = $"name = '{sanitizedName}' and trashed = false";
            listRequest.Fields = "files(id, name, mimeType, size, modifiedTime)";
            listRequest.PageSize = 1;
            listRequest.Spaces = "drive";

            // Uncomment if searching within Shared Drives (Workspace):
            listRequest.SupportsAllDrives = true;
            listRequest.IncludeItemsFromAllDrives = true;

            FileList response = await listRequest.ExecuteAsync(cancellationToken);
            Google.Apis.Drive.v3.Data.File? file = response.Files?.FirstOrDefault();

            return (file != null, file?.Id);
        }

        private (string folderTypeRoot, string folderName) GetFolderTypeRoot(int documentType)
        {
            var _driveAppsettings = _options.Value.GoogleDrive;
            int[] receptionList = { 2, 6 };
            int[] dispatchList = { 3, 4, 7 };

            return documentType switch
            {
                1 => (_driveAppsettings.ReceptionFolder, "FX"),
                8 => (_driveAppsettings.BulgetFolder, "PP"),
                int n when receptionList.Contains(n) => (_driveAppsettings.ReceptionFolder, "GR"),
                int n when dispatchList.Contains(n) => (_driveAppsettings.DispatchFolder, "GD"),
                5 => (_driveAppsettings.VoucherFolder, "VC"),
                9 => (_driveAppsettings.ExpenseFolder, "RG"),
                _ => (_driveAppsettings.SharedFolderRoot, "SharedFolderRoot")
            };
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
