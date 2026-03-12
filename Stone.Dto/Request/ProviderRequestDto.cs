namespace Stone.Dto.Request
{
    public class ProviderRequestDto
    {
        public int Id { get; set; }
        public int PersonId { get; set; }
        public int LocationId { get; set; }
        public int BankAccountId { get; set; }
        public bool Active { get; set; }
    }
}
