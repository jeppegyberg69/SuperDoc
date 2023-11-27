using System.ComponentModel.DataAnnotations;
using SuperDoc.Customer.Repositories.Entities.Users;

namespace SuperDoc.Customer.Repositories.Entities.Documents
{
    public class Revision
    {
        [Key]
        public Guid RevisionId { get; set; }
        public Guid? DocumentId { get; set; }

        [Required]
        [StringLength(256, MinimumLength = 1)]
        public string FilePath { get; set; } = string.Empty;

        [Required]
        [StringLength(128, MinimumLength = 1)]
        public string DocumentHash { get; set; } = string.Empty;

        public Guid? UserId { get; set; }

        [Required]
        public DateTime DateUploaded { get; set; }

        public virtual Document? Document { get; set; }
        public virtual User? User { get; set; }
        public virtual ICollection<DocumentSignature>? DocumentSignatures { get; set; }
    }
}
