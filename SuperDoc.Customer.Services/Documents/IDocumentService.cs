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
    }
}