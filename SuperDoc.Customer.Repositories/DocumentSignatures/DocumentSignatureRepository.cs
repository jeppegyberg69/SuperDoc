using SuperDoc.Customer.Repositories.Contexts;
using SuperDoc.Customer.Repositories.Entities.Documents;

namespace SuperDoc.Customer.Repositories.DocumentSignatures
{
    public class DocumentSignatureRepository : IDocumentSignatureRepository
    {
        private readonly SuperDocContext superDocContext;

        public DocumentSignatureRepository(SuperDocContext superDocContext)
        {
            this.superDocContext = superDocContext;
        }

        public async Task CreateDocumentSignaturesAsync(IEnumerable<DocumentSignature> documentSignatures)
        {
            await superDocContext.DocumentSignatures.AddRangeAsync(documentSignatures);
            await superDocContext.SaveChangesAsync();
        }

        public Task UpdateDocumentSignatureAsync(DocumentSignature documentSignature)
        {
            superDocContext.DocumentSignatures.Update(documentSignature);
            return superDocContext.SaveChangesAsync();
        }
    }
}
