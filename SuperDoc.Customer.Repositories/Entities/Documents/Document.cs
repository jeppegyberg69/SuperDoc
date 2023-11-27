using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SuperDoc.Customer.Repositories.Entities.Cases;
using SuperDoc.Customer.Repositories.Entities.Users;

namespace SuperDoc.Customer.Repositories.Entities.Documents
{
    [Table("Documents")]
    public class Document
    {
        [Key]
        public Guid DocumentId { get; set; }

        public Guid? CaseId { get; set; }

        [Required]
        [StringLength(256, MinimumLength = 1)]
        public string Title { get; set; } = string.Empty;

        [StringLength(1024, MinimumLength = 1)]
        public string Description { get; set; } = string.Empty;

        [Required]
        public DateTime? DateCreated { get; set; }
        [Required]
        public DateTime? DateModified { get; set; }

        public virtual Case? Case { get; set; }
        public virtual ICollection<User>? ExternalUsers { get; set; }
        public virtual ICollection<Revision>? Revisions { get; set; }

    }
}
