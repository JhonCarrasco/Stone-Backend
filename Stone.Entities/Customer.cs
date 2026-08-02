using System.ComponentModel.DataAnnotations.Schema;

namespace Stone.Entities
{
    public class Customer : EntityBase
    {
        [Column("email")]
        public string Email { get; set; }
        [Column("telefono")]
        public string Phone { get; set; }
        [Column("giro")]
        public string? BusinessActivity { get; set; }
        [Column("persona_id")]
        public int? PersonId { get; set; }
        [Column("ubicacion_id")]
        public int? LocationId { get; set; }
        [Column("cuenta_banco_id")]
        public int? BankAccountId { get; set; }       
        public virtual BankAccount? BankAccount { get; set; }
        public virtual Person? Person { get; set; }
        public virtual Location? Location { get; set; }
        public virtual List<Contact>? Contacts { get; set; }
    }
}
