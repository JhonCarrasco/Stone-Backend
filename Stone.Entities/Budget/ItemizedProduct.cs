using System.ComponentModel.DataAnnotations.Schema;

namespace Stone.Entities.Budget
{
    public class ItemizedProduct: EntityBase
    {
        [Column("descripcion")]
        public string Description { get; set; }
        [Column("largo")]
        public double Long { get; set; }
        [Column("ancho")]
        public double Width { get; set; }
        [Column("color")]
        public string Color { get; set; }
        [Column("valor_unitario")]
        public int UnitValue { get; set; }
        [Column("cantidad")]
        public int amount { get; set; }
        [Column("valor_total")]
        public int TotalValue { get; set; }
        [Column("presupuesto_id")]
        public int? BudgetId { get; set; }

        public virtual Budget Budget { get; set; }
    }
}
