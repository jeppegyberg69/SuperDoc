using SuperDoc.Customer.Repositories.Entities.Documents;
using SuperDoc.Shared.Models.Documents;

namespace SuperDoc.Customer.Services.Documents.Factories
{
    public interface IRevisionFactory
    {
        IEnumerable<DocumentSignatureDto> ConvertSignaturesToDtos(IEnumerable<DocumentSignature> signatures);
        DocumentSignatureDto ConvertSignatureToDto(DocumentSignature signature);
        RevisionDto CoonverRevisionToDto(Revision revision);
    }
}