namespace Stone.Dto.Request
{
    public class ItemizedProductRequestDto
    {
        public int? Id { get; set; }
        public string Description { get; set; }
        public decimal Long { get; set; }
        public decimal Width { get; set; }
        public decimal? Thickness { get; set; }
        public string Color { get; set; }
        public int UnitValue { get; set; }
        public int amount { get; set; }
        public int TotalValue { get; set; }
        public int? BudgetId { get; set; }
    }
}
