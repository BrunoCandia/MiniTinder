using System.ComponentModel.DataAnnotations;

namespace API.DTO
{
    public class RegisterDto
    {
        [Required]
        public string DisplayName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters long.")]
        public string Password { get; set; } = string.Empty;

        // The required keyword does not work with validations
        //[Required]
        //public required string DisplayName { get; set; }

        //[Required]
        //[EmailAddress]
        //public required string Email { get; set; }

        //[Required]
        //[MinLength(6, ErrorMessage = "Password must be at least 6 characters long.")]        
        //public required string Password { get; set; }
    }
}
