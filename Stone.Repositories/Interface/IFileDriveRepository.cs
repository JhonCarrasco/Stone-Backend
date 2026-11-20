using Stone.Entities;

namespace Stone.Repositories.Interface
{
    public interface IFileDriveRepository : IRepositoryBase<FileDrive>
    {
        Task<bool> DeleteByFileIdAsync(string fileId);
    }

}
