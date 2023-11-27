using System;
using System.ComponentModel.DataAnnotations;

namespace SuperDoc.Shared.Models.Documents
{
    public class CreateOrUpdateDocumentDto
    {
        public Guid? DocumentId { get; set; }
        public Guid? CaseId { get; set; }

        [Required]
        [StringLength(256, MinimumLength = 1)]
        public string Title { get; set; } = string.Empty;

        [MaxLength(1024)]
        public string? Description { get; set; } = string.Empty;
    }
}
