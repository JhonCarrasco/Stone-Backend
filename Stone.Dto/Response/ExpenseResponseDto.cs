namespace Stone.Dto.Response
{
    public class ExpenseResponseDto
    {
        public int Id { get; set; }
        public bool Active { get; set; }
        public DateTime? CreateAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int ExpenseTypeId { get; set; }
        public DateTime ExpenseDate { get; set; }
        public string Employee { get; set; }
        public string? Project { get; set; }
        public int? BudgetId { get; set; }
        public string? File { get; set; }
        public int Amount { get; set; }
        public int? MethodPaymentId { get; set; }
        public int? PaymentReceiptId { get; set; }
        public string? BillNumber { get; set; }
        public string? Description { get; set; }
        public string? Vehicle { get; set; }
        public string? Registration { get; set; }
        public string? Location { get; set; }
    }
}
