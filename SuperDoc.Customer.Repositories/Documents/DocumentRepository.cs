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
    }
}
