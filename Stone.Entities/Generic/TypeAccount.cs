using System.ComponentModel.DataAnnotations.Schema;

namespace Stone.Entities.Generic
{
    public class TypeAccount : EntityBase
    {
        [Column("descripcion")]
        public string? Description { get; set; }
    }
}
