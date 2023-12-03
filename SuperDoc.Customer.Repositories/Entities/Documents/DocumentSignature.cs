using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SuperDoc.Customer.Repositories.Entities.Users;

namespace SuperDoc.Customer.Repositories.Entities.Documents
{
    [Table("DocumentSignatories")]
    public class DocumentSignature
    {
        [Key]
        public Guid SignatureId { get; set; }
        public Guid RevisionId { get; set; }
        public Guid UserId { get; set; }

        [Required]
        [StringLength(736, MinimumLength = 1)]
        public string PublicKey { get; set; } = string.Empty;

        [Required]
        [StringLength(2000, MinimumLength = 1)]
        public string Signature { get; set; } = string.Empty;

        public DateTime? DateSigned { get; set; }

        public virtual Revision? Revision { get; set; }
        public virtual User? User { get; set; }
    }
}
