using SuperDoc.Customer.Repositories.Entities.Documents;

namespace SuperDoc.Customer.Repositories.Documents
{
    public interface IDocumentRepository
    {
        Task<Guid> CreateDocumentAsync(Document document);
        Task<Document?> GetDocumentByIdAsync(Guid documentId);
        Task<Document?> GetDocumentByIdWithExternialUsersAsync(Guid documentId);
        Task UpdateDocumentAsync(Document document);
    }
}