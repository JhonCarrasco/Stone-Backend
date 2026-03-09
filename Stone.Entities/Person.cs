using System.ComponentModel.DataAnnotations.Schema;

namespace Stone.Entities
{
    public class Person: EntityBase
    {
        [Column("rut")]
        public string Rut { get; set; }
        [Column("nombre_persona")]
        public string DisplayName { get; set; }
        [Column("tipo_persona")]
        public int? TypePerson { get; set; }
    }
}
