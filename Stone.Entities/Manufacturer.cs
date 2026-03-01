using System.ComponentModel.DataAnnotations.Schema;

namespace Stone.Entities
{
    public class Manufacturer: EntityBase
    {
        [Column("descripcion")]
        public string Description { get; set; }
    }
}
