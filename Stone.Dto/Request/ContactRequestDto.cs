namespace Stone.Dto.Request
{
    public class ContactRequestDto
    {
        public int? Id { get; set; }        
        public bool? Active { get; set; }
        public string DisplayName { get; set; }
        public string Phone { get; set; }
        public string? Email { get; set; }
        public string? Position { get; set; }
        public int? CustomerId { get; set; }        
        public int? ProviderId { get; set; }
    }
}
