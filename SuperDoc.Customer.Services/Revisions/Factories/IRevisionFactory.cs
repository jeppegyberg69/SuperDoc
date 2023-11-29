using SuperDoc.Customer.Repositories.Entities.Documents;
using SuperDoc.Shared.Models.Documents;
using SuperDoc.Shared.Models.Revisions;

namespace SuperDoc.Customer.Services.Revisions.Factories
{
    public interface IRevisionFactory
    {
        IEnumerable<DocumentSignatureDto> ConvertSignaturesToDtos(IEnumerable<DocumentSignature> signatures);
        DocumentSignatureDto ConvertSignatureToDto(DocumentSignature signature);
        RevisionDto ConvertRevisionToDto(Revision revision);
        IEnumerable<RevisionDto> ConvertRevisionsToDtos(IEnumerable<Revision> revisions);
    }
}