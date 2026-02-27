using System.ComponentModel.DataAnnotations;

namespace Stone.Dto.Request
{
    public class RegisterRequestDto
    {
        [Required]
        [StringLength(200)]
        public string FirstName { get; set; } = default!;

        [Required]
        [StringLength(200)]

        public string LastName { get; set; } = default!;
        [StringLength(200)]
        public string MiddleName { get; set; }

        [StringLength(200)]
        [EmailAddress]
        public string Email { get; set; } = default!;

        [StringLength(20)]
        [Required]
        public string Rut { get; set; } = default!;
        
        [StringLength(200)]
        public string LastNameMother { get; set; }


        [Required]
        public string Password { get; set; } = default!;

        [Compare(nameof(Password), ErrorMessage = "Las contraseñas deben coincidir.")]
        public string ConfirmPassword { get; set; } = default!;
    }
}
