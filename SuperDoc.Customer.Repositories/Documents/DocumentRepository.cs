using Microsoft.EntityFrameworkCore;
using SuperDoc.Customer.Repositories.Contexts;
using SuperDoc.Customer.Repositories.Entities.Documents;

namespace SuperDoc.Customer.Repositories.Documents
{
    public class DocumentRepository : IDocumentRepository
    {
        private readonly SuperDocContext superDocContext;

        public DocumentRepository(SuperDocContext superDocContext)
        {
            this.superDocContext = superDocContext;
        }

        public async Task<Document?> GetDocumentByIdAsync(Guid documentId)
        {
            return await superDocContext.Documents.FirstOrDefaultAsync(x => x.DocumentId == documentId);
        }

        public async Task<Document?> GetDocumentByIdWithExternialUsersAsync(Guid documentId)
        {
            return await superDocContext.Documents.Include(x => x.ExternalUsers).FirstOrDefaultAsync(x => x.DocumentId == documentId);
        }

        public async Task<Guid> CreateDocumentAsync(Document document)
        {
            await superDocContext.Documents.AddAsync(document);
            await superDocContext.SaveChangesAsync();
            return document.DocumentId;
        }

        public async Task UpdateDocumentAsync(Document document)
        {
            superDocContext.Documents.Update(document);
            await superDocContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Document>> GetDocumentsByCaseIdWithExternalUsersAsync(Guid caseId)
        {
            return await superDocContext.Documents.Where(x => x.CaseId == caseId).Include(x => x.ExternalUsers).ToListAsync();
        }

        public async Task<IEnumerable<Document>> GetDocumentsByCaseIdAndUserIdWithExternalUsersAsync(Guid caseId, Guid userId)
        {
#nullable disable
            var documents = await superDocContext.Documents
                .Where(doc => doc.CaseId == caseId && doc.ExternalUsers.Any(user => user.UserId == userId))
                .Include(doc => doc.ExternalUsers)
                .ToListAsync();
#nullable enable
            return documents;
        }
    }
}
