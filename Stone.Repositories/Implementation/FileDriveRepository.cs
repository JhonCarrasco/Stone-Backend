using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Stone.Entities;
using Stone.Persistence;
using Stone.Repositories.Implements;
using Stone.Repositories.Interface;

namespace Stone.Repositories.Implementation
{
    public class FileDriveRepository : RepositoryBase<FileDrive>, IFileDriveRepository
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public FileDriveRepository(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor) : base(context)
        {
            this._httpContextAccessor = httpContextAccessor;
        }

        public async Task<bool> DeleteByFileIdAsync(string fileId)
        {
            if (string.IsNullOrWhiteSpace(fileId))
                return false;

            var queryable = context.Set<FileDrive>()
                .Where(m => m.FileId == fileId && m.Active)
                .AsQueryable();

            var response = await queryable.ExecuteDeleteAsync();
            return response > 0;
        }

    }
}
