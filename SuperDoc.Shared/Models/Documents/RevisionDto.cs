

namespace SuperDoc.Shared.Models.Documents
{
    public class RevisionDto
    {
        public Guid RevisionId { get; set; }
        public DateTime DateUploaded { get; set; }
        public IEnumerable<DocumentSignatureDto> Signatures { get; set; } = new List<DocumentSignatureDto>();
    }
}
