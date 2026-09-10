using Microsoft.AspNetCore.Http;
using Stone.Dto.Response;

namespace Stone.Services.Interface
{
    public interface IGoogleDriveService
    {
        Task<BaseResponseGeneric<List<FileResponse>>> GetFiles(string folderId);
        Task<BaseResponseGeneric<string>> CreateFolder(string parentId, string folderName);
        Task<BaseResponseGeneric<string>> UploadToFolder(IFormFile file, string folderId);
        Task<BaseResponseGeneric<string>> DownloadFile(string fileId);
    }
}
