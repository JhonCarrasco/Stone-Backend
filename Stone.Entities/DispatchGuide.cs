using System.ComponentModel.DataAnnotations.Schema;

namespace Stone.Entities
{
    public class DispatchGuide : EntityBase
    {
        [Column("folio")]
        public string Folio { get; set; }              
        [Column("tipo_documento")]
        public int? DocumentType { get; set; }//guia de despacho, guia de devolucion
        [Column("observaciones")]
        public string? Observations { get; set; }
        [Column("tipo_moneda")]
        public string? CurrencyType { get; set; }
        [Column("valor_moneda")]
        public int? ValueCurrency { get; set; }
        [Column("proveedor_id")]
        public int? ProviderId { get; set; }
        [Column("cliente_id")]
        public int? CustomerId { get; set; }
        [Column("neto")]
        public int? Neto { get; set; }
        [Column("iva", TypeName = "decimal(3,2)")]
        public decimal? TaxRate { get; set; }
        [Column("valor_total")]
        public int? TotalValue { get; set; }       
        [Column("ubicacion_id")]
        public int? LocationId { get; set; }
        [Column("fecha_despacho")]
        public DateTime? GuideDate { get; set; }
        public virtual ICollection<Material>? Materials { get; set; }

        public virtual Customer? Customer { get; set; }
        public virtual Provider? Provider { get; set; }


    }
}
