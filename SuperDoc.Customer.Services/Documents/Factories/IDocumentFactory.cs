using SuperDoc.Customer.Repositories.Entities.Documents;
using SuperDoc.Shared.Models.Documents;

namespace SuperDoc.Customer.Services.Documents.Factories
{
    public interface IDocumentFactory
    {
        IEnumerable<DocumentDto> CreateDocumentDtos(IEnumerable<Document> documents);
    }
}