using System.ComponentModel.DataAnnotations.Schema;

namespace Stone.Entities
{
    public class Budget: EntityBase
    {
        [Column("cliente_nombre")]
        public string? CustomerName { get; set; }
        [Column("proyecto_nombre")]
        public string? ProjectName { get; set; }
        [Column("direccion")]
        public string? Address { get; set; }
        [Column("descripcion")]
        public string? Description { get; set; }
      
        [Column("material")]
        public string? Material { get; set; }
        [Column("subtotal")]
        public int SubTotal { get; set; }
        [Column("neto")]
        public int Neto { get; set; }
        [Column("iva")]
        public int Iva { get; set; }
        [Column("valor_total")]
        public int TotalValue { get; set; }
        [Column("cliente_id")]
        public int? CustomerId { get; set; }

        public virtual Customer? Customer { get; set; }



        public virtual ICollection<ItemizedProduct> ItemizedProducts { get; set; }
        public virtual ICollection<ItemizedService> ItemizedServices { get; set; }
        //public virtual List<ItemizedProduct>? ItemizedProducts { get; set; }
        //public virtual List<ItemizedService>? ItemizedServices { get; set; }

    }
}
