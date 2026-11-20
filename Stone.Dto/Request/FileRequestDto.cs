using Microsoft.AspNetCore.Http;

namespace Stone.Dto.Request
{
    public class FileRequestDto
    {
        public string? FolderId { get; set; }
        public int DocumentId   { get; set; }
        public int DocumentType { get; set; }
        public string? Rut { get; set; }
        public List<IFormFile>? Files { get; set; }
    }
}
