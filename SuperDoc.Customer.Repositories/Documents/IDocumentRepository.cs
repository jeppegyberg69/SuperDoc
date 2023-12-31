﻿using SuperDoc.Customer.Repositories.Entities.Documents;

namespace SuperDoc.Customer.Repositories.Documents
{
    public interface IDocumentRepository
    {
        Task<Guid> CreateDocumentAsync(Document document);
        Task<Document?> GetDocumentByIdAsync(Guid documentId);
        Task<Document?> GetDocumentByIdWithExternialUsersAsync(Guid documentId);
        Task<IEnumerable<Document>> GetDocumentsByCaseIdAndUserIdWithExternalUsersAsync(Guid caseId, Guid userId);
        Task<IEnumerable<Document>> GetDocumentsByCaseIdWithExternalUsersAsync(Guid caseId);
        Task UpdateDocumentAsync(Document document);
    }
}