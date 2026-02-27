using System.ComponentModel.DataAnnotations.Schema;

namespace Stone.Entities.Generic
{
    public class Location : EntityBase
    {
        [Column("direccion")]
        public string Address { get; set; }
        [Column("comuna_id")]
        public int CommuneId { get; set; }
        [Column("region_id")]
        public int RegionId { get; set; }

        public virtual Commune? Commune { get; set; }
        public virtual Region? Region { get; set; }
    }
}
