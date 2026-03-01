using System.ComponentModel.DataAnnotations.Schema;

namespace Stone.Entities
{
    public class Commune : EntityBase
    {
        [Column("descripcion")]
        public string Description { get; set; }
    }
}
