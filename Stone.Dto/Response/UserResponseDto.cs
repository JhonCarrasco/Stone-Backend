namespace Stone.Dto.Response
{
    public class UserResponseDto
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public bool IsActive { get; set; }
        public ICollection<string>? Roles { get; set; }
    }
}
