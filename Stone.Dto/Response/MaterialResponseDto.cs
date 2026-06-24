using Stone.Entities;

namespace Stone.Dto.Response
{
    public class MaterialResponseDto
    {
        public int? Id { get; set; }
        public bool Active { get; set; }
        public DateTime? CreateAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public string Folio { get; set; }
        public int? DocumentType { get; set; }//FX, Guia
        public string? Observations { get; set; }
        public int? CurrencyType { get; set; }
        public int? ValueCurrency { get; set; }
        //public int? ProviderId { get; set; }
        //public int? CustomerId { get; set; }
        public CustomerResponseDto? Customer { get; set; }
        public ProviderResponseDto? Provider { get; set; }
        public int? Neto { get; set; }
        public decimal? TaxRate { get; set; }
        public int? TotalValue { get; set; }
        public DateTime? GuideDate { get; set; }

        public ICollection<Material>? Materials { get; set; }



        public string? SupplierTo { get; set; }
        public string? ProjectTo { get; set; }
        public int? BudgetId { get; set; }
        public string? File { get; set; } //TODO: Cambiar a tipo de dato adecuado para archivos (e.g., IFormFile)
        public int? LocationId { get; set; }
        public string? Address { get; set; }
        public string? Zone { get; set; }
        
    }
}
