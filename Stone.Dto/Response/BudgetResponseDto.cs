using Stone.Dto.Request;

namespace Stone.Dto.Response
{
    public class BudgetResponseDto
    {
        public int Id { get; set; }
        public bool Active { get; set; }
        public DateTime? CreateAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string? ProjectName { get; set; }
        public string? Address { get; set; }
        public string? Description { get; set; }
        public string? Material { get; set; }
        public int SubTotal { get; set; }
        public int Neto { get; set; }
        public decimal TaxRate { get; set; }
        public int TotalValue { get; set; }
        public int? CustomerId { get; set; }

        public CustomerResponseDto? Customer { get; set; }
        public ICollection<ItemizedProductResponseDto>? ItemizedProducts { get; set; }
        public ICollection<ItemizedServiceResponseDto>? ItemizedServices { get; set; }
    }
}
