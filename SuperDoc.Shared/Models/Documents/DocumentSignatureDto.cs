
namespace SuperDoc.Shared.Models.Documents
{
    public class DocumentSignatureDto
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string EmailAddress { get; set; } = string.Empty;
        public string PublicKey { get; set; } = string.Empty;
        public string Signature { get; set; } = string.Empty;
        public DateTime? DateSigned { get; set; }
    }
}
