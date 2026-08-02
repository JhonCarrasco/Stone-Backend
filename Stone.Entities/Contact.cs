using System.ComponentModel.DataAnnotations.Schema;

namespace Stone.Entities
{
    public class Contact : EntityBase
    {
        [Column("nombre_contacto")]
        public string DisplayName { get; set; }
        [Column("telefono")]
        public string Phone { get; set; }
        [Column("email")]
        public string Email { get; set; }        
        [Column("cargo")]
        public string Position { get; set; }
        [Column("cliente_id")]        
        public int? CustomerId { get; set; }        
        [Column("proveedor_id")]
        public int? ProviderId { get; set; }
        public virtual Customer? Customer { get; set; }
        public virtual Provider? Provider { get; set; }
    }
}
