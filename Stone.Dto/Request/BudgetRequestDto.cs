namespace Stone.Dto.Request
{
    public class BudgetRequestDto
    {
        public int? Id { get; set; }
        public string? ProjectName { get; set; }
        public string? Address { get; set; }
        public string? Description { get; set; }
        public string? Material { get; set; }
        public int SubTotal { get; set; }
        public int Neto { get; set; }
        public decimal TaxRate { get; set; }
        public int TotalValue { get; set; }
        public int CustomerId { get; set; }

        public ICollection<ItemizedProductRequestDto>? ItemizedProducts { get; set; }
        public ICollection<ItemizedServiceRequestDto>? ItemizedServices { get; set; }
    }
}
