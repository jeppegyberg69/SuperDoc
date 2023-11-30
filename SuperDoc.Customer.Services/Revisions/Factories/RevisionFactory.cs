using SuperDoc.Customer.Repositories.Entities.Documents;
using SuperDoc.Shared.Models.Documents;
using SuperDoc.Shared.Models.Revisions;

namespace SuperDoc.Customer.Services.Revisions.Factories
{
    public class RevisionFactory : IRevisionFactory
    {


        public IEnumerable<RevisionDto> ConvertRevisionsToDtos(IEnumerable<Revision> revisions)
        {
            List<RevisionDto> result = new List<RevisionDto>();

            if (revisions != null)
            {
                foreach (var revision in revisions)
                {
                    result.Add(ConvertRevisionToDto(revision));
                }
            }

            return result;
        }

        public RevisionDto ConvertRevisionToDto(Revision revision)
        {
            return new RevisionDto
            {
                RevisionId = revision.RevisionId,
                DateUploaded = revision.DateUploaded,
                Signatures = revision.DocumentSignatures != null ? ConvertSignaturesToDtos(revision.DocumentSignatures) : new List<DocumentSignatureDto>()
            };
        }

        public IEnumerable<DocumentSignatureDto> ConvertSignaturesToDtos(IEnumerable<DocumentSignature> signatures)
        {
            var signatureDtos = new List<DocumentSignatureDto>();

            if (signatures != null)
            {
                foreach (var signature in signatures)
                {
                    signatureDtos.Add(ConvertSignatureToDto(signature));
                }
            }

            return signatureDtos;
        }

        public DocumentSignatureDto ConvertSignatureToDto(DocumentSignature signature)
        {
            return new DocumentSignatureDto
            {
                FirstName = signature.User?.FirstName == null ? string.Empty : signature.User.FirstName,
                LastName = signature.User?.LastName == null ? string.Empty : signature.User.LastName,
                EmailAddress = signature.User?.EmailAddress == null ? string.Empty : signature.User.EmailAddress,
                PublicKey = signature.PublicKey,
                Signature = signature.Signature,
                DateSigned = signature.DateSigned,
            };
        }
    }
}
