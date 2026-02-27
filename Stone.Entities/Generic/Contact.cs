using System.ComponentModel.DataAnnotations.Schema;

namespace Stone.Entities.Generic
{
    public class Contact : EntityBase
    {
        [Column("cargo")]
        public string Role { get; set; }
        [Column("telefono")]
        public string Phone { get; set; }
        [Column("email")]
        public string Email { get; set; }
        [Column("cliente_id")]        
        public int CustomerId { get; set; }
        [Column("persona_id")]
        public int PersonaId { get; set; }

        public virtual Customer? Customer { get; set; }
        public virtual Person? Person { get; set; }
    }
}
