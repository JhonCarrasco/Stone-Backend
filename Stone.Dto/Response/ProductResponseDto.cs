using Stone.Entities;

namespace Stone.Dto.Response
{
    public class ProductResponseDto
    {
        public int Id { get; set; }
        public bool Active { get; set; }
        public DateTime? CreateAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string Description { get; set; }
        public decimal? Long { get; set; }
        public decimal? Width { get; set; }
        public decimal? Thickness { get; set; }
        public string? Color { get; set; }
        public string? UnitMeasurement { get; set; }
        public int? UnitValue { get; set; }
        //public int? ManufacturerId { get; set; }
        //public int? CategoryId { get; set; }
        //public int? ProviderId { get; set; }

        public Manufacturer? Manufacturer { get; set; }
        public Category? Category { get; set; }
        public ProviderResponseDto? Provider { get; set; }
    }
}
