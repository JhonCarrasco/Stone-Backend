using System.ComponentModel.DataAnnotations.Schema;

namespace Stone.Entities
{
    public class Bank : EntityBase
    {
        [Column("descripcion")]
        public string? Description { get; set; }
    }
}
