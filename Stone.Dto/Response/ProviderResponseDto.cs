namespace Stone.Dto.Response
{
    public class ProviderResponseDto
    {
        public int Id { get; set; }
        public bool Active { get; set; }
        public DateTime? CreateAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string? PersonName { get; set; }
        public int PersonId { get; set; }
        public int? LocationId { get; set; }
        public int? BankAccountId { get; set; }
        public ICollection<ContactResponseDto>? Contacts { get; set; }

        public static implicit operator ProviderResponseDto(BaseResponseGeneric<ProviderResponseDto> v)
        {
            throw new NotImplementedException();
        }
    }
}
