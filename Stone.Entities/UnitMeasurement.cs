using System.ComponentModel.DataAnnotations.Schema;

namespace Stone.Entities
{
    public class UnitMeasurement: EntityBase
    {
        [Column("descripcion")]
        public string Description { get; set; }
    }
}
