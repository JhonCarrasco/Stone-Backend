using System.ComponentModel.DataAnnotations.Schema;

namespace Stone.Entities
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
        [Column("proveedor_id")]
        public int? ProviderId { get; set; }

        public virtual Customer? Customer { get; set; }
        public virtual Person? Person { get; set; }
        public virtual Provider Provider { get; set; }
    }
}
