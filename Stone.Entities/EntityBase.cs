using System.ComponentModel.DataAnnotations.Schema;

namespace Stone.Entities
{
    public class EntityBase
    {
        [Column("id")]
        public int Id { get; set; }
        [Column("activo")]
        public bool Active { get; set; } = true;
        [Column("fecha_creacion")]
        public DateTime? CreateAt { get; set; }
        [Column("fecha_actualizacion")]
        public DateTime? UpdatedAt { get; set; }
    }
}
