using System.ComponentModel.DataAnnotations.Schema;

namespace Stone.Entities
{
    public class ReceptionGuide : EntityBase
    {
        [Column("folio")]
        public string Folio { get; set; }        
        [Column("tipo_documento")]
        public int? DocumentType { get; set; }//FX, Guia
        [Column("observaciones")]
        public string? Observations { get; set; }
        [Column("tipo_moneda")]
        public int? CurrencyType { get; set; }
        [Column("valor_moneda", TypeName = "decimal(10,2)")]
        public decimal? ValueCurrency { get; set; }
        [Column("proveedor_id")]
        public int? ProviderId { get; set; }
        [Column("cliente_id")]
        public int? CustomerId { get; set; }
        [Column("neto", TypeName = "decimal(10,2)")]
        public decimal? Neto { get; set; }
        [Column("iva", TypeName = "decimal(10,2)")]
        public decimal? TaxRate { get; set; }
        [Column("valor_total", TypeName = "decimal(10,2)")]
        public decimal? TotalValue { get; set; }
        public virtual ICollection<Material>? Materials { get; set; }

        public string? File { get; set; }
        [Column("fecha_recepcion")]
        public DateTime? GuideDate { get; set; }

        public virtual Customer? Customer { get; set; }
        public virtual Provider? Provider { get; set; }

    }
}
