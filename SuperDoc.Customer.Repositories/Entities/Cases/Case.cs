using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SuperDoc.Customer.Repositories.Entities.Users;

namespace SuperDoc.Customer.Repositories.Entities.Cases
{
    [Table("Cases")]
    public class Case
    {
        [Key]
        [Required]
        public Guid CaseId { get; set; }

        [Required]
        [StringLength(256, MinimumLength = 1)]
        public string Title { get; set; } = string.Empty;

        [StringLength(1024, MinimumLength = 1)]
        public string Description { get; set; } = string.Empty;

        [Required]
        public bool IsArchived { get; set; }

        [Required]
        public DateTime DateModified { get; set; }

        [Required]
        public DateTime DateCreated { get; set; }

        public Guid? ResponsibleUserId { get; set; }

        public virtual User? ResponsibleUser { get; set; }

        public virtual ICollection<User>? CaseManagers { get; set; }
    }
}
