using SuperDoc.Customer.Repositories.Entities.Documents;

namespace SuperDoc.Customer.Repositories.DocumentSignatures
{
    public interface IDocumentSignatureRepository
    {
        Task CreateDocumentSignaturesAsync(IEnumerable<DocumentSignature> documentSignatures);
        Task UpdateDocumentSignatureAsync(DocumentSignature documentSignature);
    }
}