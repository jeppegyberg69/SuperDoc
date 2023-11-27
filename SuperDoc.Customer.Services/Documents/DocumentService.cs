using SuperDoc.Customer.Repositories.Cases;
using SuperDoc.Customer.Repositories.Documents;
using SuperDoc.Customer.Repositories.Entities.Cases;
using SuperDoc.Customer.Repositories.Entities.Documents;
using SuperDoc.Shared.Models.Documents;

namespace SuperDoc.Customer.Services.Documents
{
    public class DocumentService : IDocumentService
    {
        private readonly IDocumentRepository documentRepository;
        private readonly ICaseRepository caseRepository;

        public DocumentService(IDocumentRepository documentRepository, ICaseRepository caseRepository)
        {
            this.documentRepository = documentRepository;
            this.caseRepository = caseRepository;
        }


        public async Task<Guid?> CreateOrUpdateDocumentAsync(CreateOrUpdateDocumentDto documentDto)
        {
            // Update
            if (documentDto.DocumentId.HasValue)
            {
                Document? document = await documentRepository.GetDocumentByIdAsync(documentDto.DocumentId.Value);

                if (document is null)
                {
                    return null;
                }

                document.Title = documentDto.Title;
                document.Description = documentDto.Description ?? string.Empty;
                document.DateModified = DateTime.UtcNow;

                await documentRepository.UpdateDocumentAsync(document);

                return document.DocumentId;
            }
            // Create
            else
            {
                if (!documentDto.CaseId.HasValue)
                {
                    return null;
                }

                Case? caseDB = await caseRepository.GetCaseByIdAsync(documentDto.CaseId.Value);

                if (caseDB == null)
                {
                    return null;
                }

                var document = new Document
                {
                    Title = documentDto.Title,
                    Description = documentDto.Description ?? string.Empty,
                    DateCreated = DateTime.UtcNow,
                    DateModified = DateTime.UtcNow,
                    Case = caseDB
                };

                await documentRepository.CreateDocumentAsync(document);

                return document.DocumentId;
            }
        }
    }
}
