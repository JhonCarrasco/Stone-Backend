using Stone.Entities;

namespace Stone.Dto.Response
{
    public class CustomerResponseDto
    {
        public int Id { get; set; }
        public bool? Active { get; set; }
        public DateTime? CreateAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string Rut { get; set; }
        public string Email { get; set; }
        public string DisplayName { get; set; }
        public string Phone { get; set; }

        public Person? Person { get; set; }
        public Location? Location { get; set; }
        public BankAccount? BankAccount { get; set; }
        

        public ICollection<ContactResponseDto>? Contacts { get; set; }

        
    }
}
