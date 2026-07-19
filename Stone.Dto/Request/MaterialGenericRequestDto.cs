using Stone.Entities;

namespace Stone.Dto.Request
{
    public class MaterialGenericRequestDto
    {
        public int? Id { get; set; }
        public bool? Active { get; set; }
        public DateTime? CreateAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public string? Folio { get; set; }        
        public int? DocumentType { get; set; }//FX, Guia
        public string? Observations { get; set; }
        public int? CurrencyType { get; set; } // tipo de moneda
        public decimal? ValueCurrency { get; set; } // valor moneda
        public int? ProviderId { get; set; }
        public int? CustomerId { get; set; }
        public decimal? Neto { get; set; }
        public decimal? TaxRate { get; set; }
        public decimal? TotalValue { get; set; }    
        public ICollection<MaterialRequestDto>? Materials { get; set; }



        public string? SupplierTo { get; set; }
        public string? ProjectTo { get; set; }
        public int? BudgetId { get; set; }
        public string? File { get; set; } //TODO: Cambiar a tipo de dato adecuado para archivos (e.g., IFormFile)
        public int? LocationId { get; set; }
        public string? Address { get; set; }
        public int? Commune { get; set; }
        public int? Zone { get; set; }
        public DateTime? GuideDate { get; set; }
    }
}
