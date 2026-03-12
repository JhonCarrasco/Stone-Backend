using System.ComponentModel.DataAnnotations.Schema;

namespace Stone.Entities
{
    public class ItemizedProduct: EntityBase
    {
        [Column("descripcion")]
        public required string Description { get; set; }
        [Column("largo", TypeName = "decimal(8,3)")]
        public decimal? Long { get; set; }
        [Column("ancho", TypeName = "decimal(8,3)")]
        public decimal? Width { get; set; }
        [Column("espesor", TypeName = "decimal(8,3)")]
        public decimal? Thickness { get; set; }
        [Column("color")]
        public string? Color { get; set; }
        [Column("material")]
        public string? Material { get; set; }
        [Column("valor_unitario")]
        public int? UnitValue { get; set; }
        [Column("cantidad")]
        public int? amount { get; set; }
        [Column("valor_total")]
        public int? TotalValue { get; set; }
        [Column("presupuesto_id")]
        public int? BudgetId { get; set; }

        public virtual Budget? Budget { get; set; }
    }
}
