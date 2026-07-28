using System.ComponentModel.DataAnnotations.Schema;

namespace Stone.Entities
{
    public class BankAccount : EntityBase
    {
        [Column("banco_id")]
        public int BankId { get; set; }
        [Column("numero_cuenta")]
        public string AccountNumber { get; set; }
        [Column("tipo_cuenta_id")]
        public int TypeAccountId { get; set; }

        public virtual TypeAccount? TypeAccount { get; set; }
            public virtual Bank? Bank { get; set; }
    }
}
