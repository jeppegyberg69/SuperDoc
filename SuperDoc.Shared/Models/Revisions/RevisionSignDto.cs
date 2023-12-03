namespace SuperDoc.Shared.Models.Revisions
{
    public class RevisionSignDto
    {
        public Guid RevisionId { get; set; }
        public string PublicKey { get; set; } = string.Empty;
        public String Signature { get; set; } = string.Empty;
    }
}
