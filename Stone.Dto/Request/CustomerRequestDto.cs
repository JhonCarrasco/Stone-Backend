namespace Stone.Dto.Request
{
    public class CustomerRequestDto
    {
        public int? Id { get; set; }
        public string Rut { get; set; }
        public string Email { get; set; } 
        public string DisplayName { get; set; }
        public string? Phone { get; set; }
        public int? TypePerson { get; set; }
        public string? BusinessActivity { get; set; }

        #region BankAccount
        public int? BankAccountId { get; set; }
        public int? BankId { get; set; }
        public string? AccountNumber { get; set; }
        public int? TypeAccountId { get; set; }
        #endregion

        #region Location
        public int? locationId { get; set; }
        public string Address { get; set; }
        public int? RegionId { get; set; }
        public int? CommuneId { get; set; }
        #endregion

        public ICollection<ContactRequestDto>? Contacts { get; set; }
    }
}
