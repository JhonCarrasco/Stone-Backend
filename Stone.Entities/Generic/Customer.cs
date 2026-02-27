using System.ComponentModel.DataAnnotations.Schema;

namespace Stone.Entities.Generic
{
    public class Customer : EntityBase
    {
        [Column("rut")]
        public string Rut { get; set; }
        [Column("email")]
        public string Email { get; set; } = default!;
        [Column("fullname")]
        public string FullName { get; set; } = default!;
        [Column("persona_id")]
        public int PersonId { get; set; }
        [Column("ubicacion_id")]
        public int LocationId { get; set; }
        [Column("cuenta_banco_id")]
        public int BankAccountId { get; set; }
        [Column("telefono")]
        public string Phone { get; set; } = default!;

        public virtual BankAccount? BankAccount { get; set; }
        public virtual Person? Person { get; set; }
        public virtual Location? Location { get; set; } = default;
        public virtual List<Contact>? Contacts { get; set; }
    }
}
