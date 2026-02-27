using Microsoft.AspNetCore.Http;
using Stone.Dto.Validations;

namespace Stone.Dto.Request
{
    public class ConcertRequestDto
    {
        public string Title { get; set; } = default!;
        public string Description { get; set; } = default!;
        public string Place { get; set; } = default!;
        public double UnitPrice { get; set; }
        public int GenreId { get; set; }
        public string DateEvent { get; set; } = default!;
        public string TimeEvent { get; set; } = default!;
        [FileSizeValidation(MaxSizeInMegabytes:1)]
        [FileTypeValidation(FileTypeGroup.Image)]
        public IFormFile? Image { get; set; }
        public int TicketsQuantity { get; set; }
    }
}
