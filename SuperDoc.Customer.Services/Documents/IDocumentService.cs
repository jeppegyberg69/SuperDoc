using SuperDoc.Customer.Repositories.Entities.Documents;
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


    }
}