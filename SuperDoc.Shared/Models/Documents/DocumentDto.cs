using SuperDoc.Shared.Models.Cases;

namespace SuperDoc.Shared.Models.Documents
{
    public class DocumentDto
    {

        public Guid DocumentId { get; set; }
        public Guid CaseId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public IEnumerable<CaseManagerDto> ExternalUsers { get; set; } = new List<CaseManagerDto>();
    }
}
