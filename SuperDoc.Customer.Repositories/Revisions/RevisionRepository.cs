using Microsoft.EntityFrameworkCore;
using SuperDoc.Customer.Repositories.Contexts;
using SuperDoc.Customer.Repositories.Entities.Documents;

namespace SuperDoc.Customer.Repositories.Revisions
{
    public class RevisionRepository : IRevisionRepository
    {
        private readonly SuperDocContext superDocContext;

        public RevisionRepository(SuperDocContext superDocContext)
        {
            this.superDocContext = superDocContext;
        }

        public async Task<IEnumerable<Revision>> GetRevisionsByDocumentIdWithDocumentSignaturesAndUsersAsync(Guid documentId)
        {
#nullable disable
            return await superDocContext.Revisions.Where(r => r.DocumentId == documentId).Include(x => x.DocumentSignatures).ThenInclude(x => x.User).OrderByDescending(x => x.DateUploaded).ToListAsync();
#nullable enable
        }

        public async Task CreateRevisionAsync(Revision revision)
        {
            await superDocContext.Revisions.AddAsync(revision);
        }
    }
}
