
namespace SuperDoc.Customer.Services.Security
{
    public interface IAccessService
    {
        /// <summary>
        ///    Checks if the user has access to the case
        /// </summary>
        /// <param name="caseId"></param>
        /// <param name="userId"></param>
        /// <returns>
        ///     Null is returned if the user or case does not exist
        /// </returns>
        Task<bool?> HasAccessToCaseAsync(Guid caseId, Guid userId);

        /// <summary>
        ///     Checks if the user has access to the document
        /// </summary>
        /// <param name="documentId"></param>
        /// <param name="userId"></param>
        /// <returns>
        ///     Null is returned if the user or document does not exist
        /// </returns>
        Task<bool?> HasAccessToDocumentAsync(Guid documentId, Guid userId);
    }
}