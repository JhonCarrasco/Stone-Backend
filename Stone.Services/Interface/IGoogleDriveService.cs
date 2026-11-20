using Microsoft.AspNetCore.Http;
using Stone.Dto.Request;
using Stone.Dto.Response;
using Stone.Entities;

namespace Stone.Services.Interface
{
    public interface IGoogleDriveService
    {
        Task<BaseResponseGeneric<List<FileResponseDto>>> GetFilesByFolderId(string folderId);
        Task<BaseResponseGeneric<string>> CreateFolder(string parentId, string folderName);
        Task<BaseResponseGeneric<ICollection<FileDrive>>> UploadToFolder(FileRequestDto request);
        Task<BaseResponseGeneric<string>> DownloadFile(string fileId);
        Task<BaseResponseGeneric<bool>> DeleteFileAsync(string fileId);
        
    }
}
