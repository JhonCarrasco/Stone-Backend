namespace Stone.Entities
{
    public class MerchandiseReceipt : EntityBase
    {
        public DateTime DateReceipt { get; set; }
        public int ProviderId { get; set; }
        public int InvoiceNumber { get; set; }

        public virtual ICollection<Product>? Product { get; set; }

    }
}
