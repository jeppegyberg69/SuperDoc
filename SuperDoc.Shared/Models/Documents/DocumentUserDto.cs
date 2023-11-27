using System.ComponentModel.DataAnnotations;

namespace SuperDoc.Shared.Models.Documents
{
    public class DocumentUserDto
    {
        [Required]
        [EmailAddress]
        public string EmailAddress { get; set; } = string.Empty;
    }
}
