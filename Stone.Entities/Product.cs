using System.ComponentModel.DataAnnotations.Schema;

namespace Stone.Entities
{
    public class Product : EntityBase
    {
        [Column("descripcion")]
        public string Description { get; set; } = default!;
        [Column("largo", TypeName = "decimal(8,3)")]
        public decimal? Long { get; set; }
        [Column("ancho", TypeName = "decimal(8,3)")]
        public decimal? Width { get; set; }
        [Column("espesor", TypeName = "decimal(8,3)")]
        public decimal? Thickness { get; set; }
        [Column("color")]
        public string? Color { get; set; } = default!;
        [Column("unidad_medida")]
        public string? UnitMeasurement { get; set; } = default!;
        [Column("valor_unitario")]
        public int? UnitValue { get; set; }

        [Column("fabricante_id")]
        public int ManufacturerId { get; set; }
        [Column("categoria_id")]
        public int? CategoryId { get; set; }
        [Column("proveedor_id")]
        public int? ProviderId { get; set; }
        public virtual Manufacturer? Manufacturer { get; set; }
        public virtual Category? Category { get; set; }
        public virtual Provider? Provider { get; set; }
    }
}
