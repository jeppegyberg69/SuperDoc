using SuperDoc.Customer.Repositories.Cases;
using SuperDoc.Customer.Repositories.Documents;
using SuperDoc.Customer.Repositories.Entities.Cases;
using SuperDoc.Customer.Repositories.Entities.Documents;
using SuperDoc.Customer.Repositories.Entities.Users;
using SuperDoc.Customer.Repositories.Users;
using SuperDoc.Shared.Models.Documents;

namespace SuperDoc.Customer.Services.Documents
{
    public class DocumentService : IDocumentService
    {
        private readonly IDocumentRepository documentRepository;
        private readonly ICaseRepository caseRepository;
        private readonly IUserRepository userRepository;

        public DocumentService(IDocumentRepository documentRepository, ICaseRepository caseRepository, IUserRepository userRepository)
        {
            this.documentRepository = documentRepository;
            this.caseRepository = caseRepository;
            this.userRepository = userRepository;
        }

        public async Task<IEnumerable<Document>> GetDocumentsByCaseIdAndUserIdWithExternalUsersAsync(Guid caseId, Guid userId)
        {
            User? user = await userRepository.GetUserByIdAsync(userId);

            if (user == null)
            {
                return new List<Document>();
            }

            if (user.Role == Roles.SuperAdmin || user.Role == Roles.Admin || user.Role == Roles.CaseManager)
            {
                return await documentRepository.GetDocumentsByCaseIdWithExternalUsersAsync(caseId);
            }
            else
            {
                return await documentRepository.GetDocumentsByCaseIdAndUserIdWithExternalUsersAsync(caseId, userId);
            }
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
