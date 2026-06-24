using System.ComponentModel.DataAnnotations.Schema;

namespace Stone.Entities
{
    public class MaterialVoucher : EntityBase
    {
        [Column("observaciones")]
        public string? Observations { get; set; }
        [Column("nombre_persona")]
        public required string SupplierTo { get; set; }
        [Column("proyecto")]
        public string? ProjectTo { get; set; }
        [Column("presupuesto_id")]
        public int? BudgetId { get; set; }
        

        public virtual ICollection<Material>? Materials { get; set; }
    }
}
