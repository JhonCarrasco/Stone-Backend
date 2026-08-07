using System.ComponentModel.DataAnnotations.Schema;

namespace Stone.Entities
{
    public class Expense : EntityBase
    {
        [Column("gasto_tipo")]
        public int ExpenseTypeId {get; set;}
        [Column("fecha_gasto")]
        public DateTime ExpenseDate {get; set;}
        [Column("empleado")]
        public string Employee {get; set;}
        [Column("proyecto")]
        public string? Project {get; set;}
        [Column("presupuesto_id")]
        public int? BudgetId {get; set;}
        [Column("archivo")]
        public string? File {get; set;}
        [Column("monto")]
        public int Amount {get; set;}
        [Column("metodo_tipo")]
        public int? MethodPaymentId {get; set;}
        [Column("pago_tipo")]
        public int? PaymentReceiptId {get; set;}
        [Column("numero_recibo")]
        public string? BillNumber {get; set;}
        [Column("descripcion")]
        public string? Description {get; set;}
        [Column("vehiculo")]
        public string? Vehicle  {get; set;}
        [Column("patente")]
        public string? Registration {get; set;}
        [Column("ubicacion")]
        public string? Location { get; set;}

        public virtual Budget? Budget { get; set; }
    }
}
