using Microsoft.AspNetCore.Http;
using SuperDoc.Customer.Repositories.Entities.Documents;
using SuperDoc.Customer.Services.Shared.StatusModels;
using SuperDoc.Shared.Models.Documents;

namespace SuperDoc.Customer.Services.Documents
{
    public interface IDocumentService
    {
        /// <summary>
        ///     This method creates or updates a document.
        /// </summary>
        /// <param name="documentDto"></param>
        /// <returns>
        ///     The guid of the created or updated document.<br></br>
        ///     Null is returned if the document could not be found or the given case id don't exist in the database.
        /// </returns>
        Task<Guid?> CreateOrUpdateDocumentAsync(CreateOrUpdateDocumentDto documentDto);

        /// <summary>
        ///    This method returns all documents for the given case id.
        /// </summary>
        /// <param name="caseId"></param>
        /// <param name="userId"></param>
        /// <returns>
        ///     The documents for the given case id.<br></br>
        ///     If the person signed in has the role User, then only the documents that are shared with the user are returned.<br></br>
        /// </returns>
        Task<IEnumerable<Document>> GetDocumentsByCaseIdAndUserIdWithExternalUsersAsync(Guid caseId, Guid userId);

        /// <summary>
        ///    This method will creae a new revision for the given document.<br></br>
        ///    And if the given email address don't exist in the database, a new user will be created.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="documentId"></param>
        /// <param name="emailAddresses"></param>
        /// <param name="documentFile"></param>
        /// <returns>
        ///     The created revision if the file was uploaded successfully.<br></br>
        ///     Revision is null if the file could not be uploaded. Read the error message for more information.
        /// </returns>
        Task<ResultModel<Revision>> SaveUploadedFile(Guid userId, Guid documentId, string emailAddresses, IFormFile documentFile);
    }
}