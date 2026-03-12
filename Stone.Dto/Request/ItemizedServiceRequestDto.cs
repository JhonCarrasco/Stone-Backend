namespace Stone.Dto.Request
{
    public class ItemizedServiceRequestDto
    {
        public int? Id { get; set; }
        public string Description { get; set; }
        public int UnitValue { get; set; }
        public int amount { get; set; }
        public int TotalValue { get; set; }
        public int? BudgetId { get; set; }
    }
}
