using SuperDoc.Customer.Repositories.Entities.Documents;

namespace SuperDoc.Customer.Repositories.Revisions
{
    public interface IRevisionRepository
    {
        Task<IEnumerable<Revision>> GetRevisionsByDocumentIdWithDocumentSignaturesAsync(Guid documentId);
    }
}