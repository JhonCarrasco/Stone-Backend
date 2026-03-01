using System.ComponentModel.DataAnnotations.Schema;

namespace Stone.Entities
{
    public class Category: EntityBase
    {
        [Column("descripcion")]
        public string Description { get; set; }
    }
}
