using System.ComponentModel.DataAnnotations.Schema;

namespace Stone.Entities.Generic
{
    public class Region : EntityBase
    {
        [Column("descripcion")]
        public string Description { get; set; }

    }
}
