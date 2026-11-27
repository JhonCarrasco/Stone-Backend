using System.ComponentModel.DataAnnotations.Schema;

namespace Stone.Entities
{
    public class Commune : EntityBase
    {
        [Column("descripcion")]
        public string Description { get; set; }
        [Column("region_id")]
        public int RegionId { get; set; }
    }
}
