namespace Stone.Dto.Response
{
    public class ContactResponseDto
    {
        public int? ContactId { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public int? PersonId { get; set; }
        public string Rut { get; set; }
        public string DisplayName { get; set; }
        public string BusinessActivity { get; set; }
        public int? ProviderId { get; set; }
        public string? ProviderName { get; set; }
    }
}
