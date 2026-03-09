namespace Stone.Dto.Request
{
    public class ContactRequestDto
    {
        public int? ContactId { get; set; }
        public string BusinessActivity { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public int? CustomerId { get; set; }
        public int? PersonId { get; set; }
        public string Rut { get; set; }
        public string DisplayName { get; set; }
        public int TypePerson { get; set; }
        public int? ProviderId { get; set; }
    }
}
