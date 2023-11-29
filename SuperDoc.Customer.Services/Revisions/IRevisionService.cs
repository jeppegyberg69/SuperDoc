using SuperDoc.Customer.Repositories.Entities.Documents;

namespace SuperDoc.Customer.Services.Revisions
{
    public interface IRevisionService
    {
        Task<IEnumerable<Revision>> GetRevisionsByDocumentIdWithDocumentSignaturesAsync(Guid documentId);
    }
}