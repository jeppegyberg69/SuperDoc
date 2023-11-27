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
        [StringLength(392, MinimumLength = 1)]
        public string PublicKey { get; set; } = string.Empty;

        [Required]
        [StringLength(128, MinimumLength = 1)]
        public string Signature { get; set; } = string.Empty;

        public virtual Revision? Revision { get; set; }
        public virtual User? User { get; set; }
    }
}
