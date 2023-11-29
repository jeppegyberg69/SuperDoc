using SuperDoc.Customer.Repositories.Entities.Documents;
using SuperDoc.Customer.Repositories.Entities.Users;
using SuperDoc.Customer.Services.Cases.Factories;
using SuperDoc.Shared.Models.Documents;

namespace SuperDoc.Customer.Services.Documents.Factories
{
    public class DocumentFactory : IDocumentFactory
    {
        private readonly ICaseFactory caseFactory;

        public DocumentFactory(ICaseFactory caseFactory)
        {
            this.caseFactory = caseFactory;
        }


        public IEnumerable<DocumentDto> CreateDocumentDtos(IEnumerable<Document> documents)
        {
            List<DocumentDto> documentDtos = new List<DocumentDto>();

            if (documents != null)
            {
                foreach (var document in documents)
                {
                    documentDtos.Add(new DocumentDto
                    {
                        DocumentId = document.DocumentId,
                        CaseId = document?.CaseId ?? Guid.NewGuid(),
                        Title = document?.Title ?? string.Empty,
                        Description = document?.Description ?? string.Empty,
                        DateCreated = document?.DateCreated,
                        DateModified = document?.DateModified,
                        ExternalUsers = caseFactory.ConverUsersToCaseManagerDtos(document?.ExternalUsers ?? new List<User>())
                    });
                }
            }

            return documentDtos;
        }
    }
}
