using SuperDoc.Shared.Models.Cases;
using SuperDoc.Shared.Models.Documents;

namespace SuperDoc.Shared.Services.Contracts;

/// <summary>
/// Service for interacting with case-related operations.
/// </summary>
/// <param name="httpClientFactory">The factory for creating an instances of <see cref="HttpClient"/>.</param>
public interface ICaseService : IHttpService
{
    /// <summary>
    /// Retrieves a collection of cases asynchronously.
    /// </summary>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> token that can be used to cancel the operation.</param>
    /// <returns> A <see cref="Task"/> representing the asynchronous operation, containing a collection of <see cref="CaseDto"/> instances if the operation is successful; otherwise, an empty collection.</returns>
    /// <exception cref="HttpServiceException">Thrown when an unexpected error occurs during the HTTP request or if the response indicates failure.</exception>
    public Task<IEnumerable<CaseDto>> GetCasesAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a collection of documents associated with a specific case asynchronously.
    /// </summary>
    /// <param name="caseId">The unique identifier of the case for which documents are requested.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> token that can be used to cancel the operation.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation, containing a collection of <see cref="DocumentDto"/> instances if the operation is successful; otherwise, an empty collection.</returns>
    /// <exception cref="HttpServiceException">Thrown when an unexpected error occurs during the HTTP request or if the response indicates failure.</exception>
    public Task<IEnumerable<DocumentDto>> GetCaseDocumentsAsync(Guid caseId, CancellationToken cancellationToken = default);
}