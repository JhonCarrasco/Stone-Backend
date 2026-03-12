namespace Stone.Dto.Response
{
    public class ItemizedProductResponseDto
    {
        public int Id { get; set; }
        public bool Active { get; set; }
        public DateTime? CreateAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public required string Description { get; set; }
        public decimal? Long { get; set; }
        public decimal? Width { get; set; }
        public decimal? Thickness { get; set; }
        public string? Color { get; set; }
        public string? Material { get; set; }
        public int? UnitValue { get; set; }
        public int? amount { get; set; }
        public int? TotalValue { get; set; }
        public int? BudgetId { get; set; }
    }
}
