using System.ComponentModel.DataAnnotations.Schema;

namespace Stone.Entities.Generic
{
    public class Bank : EntityBase
    {
        [Column("descripcion")]
        public string? Description { get; set; }
    }
}
