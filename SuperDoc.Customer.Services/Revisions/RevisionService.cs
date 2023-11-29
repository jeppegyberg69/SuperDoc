using SuperDoc.Customer.Repositories.Entities.Documents;
using SuperDoc.Customer.Repositories.Revisions;

namespace SuperDoc.Customer.Services.Revisions
{
    public class RevisionService : IRevisionService
    {
        private readonly IRevisionRepository revisionRepository;

        public RevisionService(IRevisionRepository revisionRepository)
        {
            this.revisionRepository = revisionRepository;
        }

        public async Task<IEnumerable<Revision>> GetRevisionsByDocumentIdWithDocumentSignaturesAsync(Guid documentId)
        {
            return await revisionRepository.GetRevisionsByDocumentIdWithDocumentSignaturesAsync(documentId);
        }
    }
}
