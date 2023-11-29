using SuperDoc.Shared.Models.Documents;

namespace SuperDoc.Shared.Models.Revisions
{
    public class RevisionDto
    {
        public Guid RevisionId { get; set; }
        public DateTime DateUploaded { get; set; }
        public IEnumerable<DocumentSignatureDto> Signatures { get; set; } = new List<DocumentSignatureDto>();
    }
}
