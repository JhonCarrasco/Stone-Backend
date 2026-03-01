using System.ComponentModel.DataAnnotations.Schema;

namespace Stone.Entities
{
    public class Person: EntityBase
    {
        //[Key]
        //[Column("codigo_persona")]
        //public new int Id { get; set; }

        [Column("rut")]
        public string Rut { get; set; }
        [Column("nombre")]
        public string FirstName { get; set; }
        [Column("segundo_nombre")]
        public string MiddleName { get; set; }
        [Column("paterno")]
        public string LastName { get; set; }
        [Column("materno")]
        public string LastNameMother { get; set; }
    }
}
