using SuperDoc.Customer.Repositories.Entities.Documents;

namespace SuperDoc.Customer.Repositories.Revisions
{
    public interface IRevisionRepository
    {
        Task CreateRevisionAsync(Revision revision);
        Task<Revision?> GetRevisionByIdAsync(Guid revisionId);
        Task<Revision?> GetRevisionByIdWithDocumentSignaturesAndUsersAsync(Guid revisionId);
        Task<IEnumerable<Revision>> GetRevisionsByDocumentIdWithDocumentSignaturesAndUsersAsync(Guid documentId);
    }
}