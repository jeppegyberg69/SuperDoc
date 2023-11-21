using System.ComponentModel.DataAnnotations;

namespace SuperDoc.Shared.Models
{
    public class LoginDto
    {
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [StringLength(128, MinimumLength = 1)]
        public string Password { get; set; } = string.Empty;
    }
}
