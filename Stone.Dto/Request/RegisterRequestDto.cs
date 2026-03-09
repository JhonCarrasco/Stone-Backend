using System.ComponentModel.DataAnnotations;

namespace Stone.Dto.Request
{
    public class RegisterRequestDto
    {
        [Required]
        [StringLength(200)]
        public string DisplayName { get; set; } = default!;

        [Required]
        public int TypePerson { get; set; } = default!;
        
        [StringLength(200)]
        [EmailAddress]
        public string Email { get; set; } = default!;

        [StringLength(20)]
        [Required]
        public string Rut { get; set; } = default!;
        

        [Required]
        public string Password { get; set; } = default!;

        [Compare(nameof(Password), ErrorMessage = "Las contraseñas deben coincidir.")]
        public string ConfirmPassword { get; set; } = default!;
    }
}
