using System.ComponentModel.DataAnnotations;

namespace SuperDoc.Shared.Models.Users
{
    public class CrerateUserDto
    {


        [Required]
        [StringLength(100, MinimumLength = 1)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [StringLength(100, MinimumLength = 1)]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string EmailAddress { get; set; } = string.Empty;
        public int? PhoneCode { get; set; }
        public long? PhoneNumber { get; set; }

        [StringLength(128, MinimumLength = 8)]
        public string Password { get; set; } = string.Empty;

        [Required]
        public int Role { get; set; }
    }
}
