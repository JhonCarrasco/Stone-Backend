using System.ComponentModel.DataAnnotations.Schema;

namespace Stone.Entities
{
    public class Material : EntityBase
    {
        [Column("producto_codigo")]
        public string? ProductCode { get; set; }
        [Column("descripcion")]
        public string Description { get; set; }
        [Column("unidad_medida")]
        public string? UnitMeasurement { get; set; }
        [Column("cantidad", TypeName = "decimal(3,2)")]
        public decimal? Quantity { get; set; }
        [Column("valor_unitario")]
        public int? UnitValue { get; set; }
        [Column("valor_total")]
        public int? TotalValue { get; set; }
        [Column("producto_id")]
        public int? ProductId { get; set; }
        [Column("voucher_id")]
        public int? VoucherId { get; set; }
        [Column("recepcion_id")]
        public int? ReceptionId { get; set; }
        [Column("despacho_id")]
        public int? DispatchId { get; set; }

    }
}
