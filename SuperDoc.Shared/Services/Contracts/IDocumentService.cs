using SuperDoc.Shared.Models.Documents;
using SuperDoc.Shared.Models.Revisions;

namespace SuperDoc.Shared.Services.Contracts;

/// <summary>
/// Service for interacting with document-related operations.
/// </summary>
/// <param name="httpClientFactory">The factory for creating an instances of <see cref="HttpClient"/>.</param>
public interface IDocumentService : IHttpService
{
    /// <summary>
    /// Retrieves a collection of revisions associated with a specific document.
    /// </summary>
    /// <param name="documentId">The unique identifier of the document for which revisions are requested.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> token that can be used to cancel the operation.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation, containing a collection of <see cref="RevisionDto"/> instances if the operation is successful; otherwise, an empty collection.</returns>
    /// <exception cref="HttpServiceException">Thrown when an unexpected error occurs during the HTTP request or if the response indicates failure.</exception>
    public Task<IEnumerable<RevisionDto>> GetDocumentRevisionsAsync(Guid documentId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves the document stream for a specific revision.
    /// </summary>
    /// <param name="revisionId">The unique identifier of the revision for which the document stream is requested.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> token that can be used to cancel the operation.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation, containing the document stream as a <see cref="Stream"/> or <see langword="null"/> if the revision is not found.</returns>
    /// <exception cref="HttpServiceException">Thrown when an unexpected error occurs during the HTTP request or if the response indicates failure.</exception>
    public Task<Stream?> GetDocumentStreamAsync(Guid revisionId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Signs a document revision with the provided signature and public key.
    /// </summary>
    /// <param name="revisionId">The unique identifier of the revision to be signed.</param>
    /// <param name="signature">The signature to be applied to the document revision.</param>
    /// <param name="publicKey">The public key associated with the private key used to sign the document revision.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> token that can be used to cancel the operation.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation, containing a <see cref="DocumentSignatureDto"/> with information about the signed document revision.</returns>
    /// <exception cref="HttpServiceException">Thrown when an unexpected error occurs during the HTTP request or if the response indicates failure.</exception>
    public Task<DocumentSignatureDto?> SignDocumentRevisionAsync(Guid revisionId, string signature, string publicKey, CancellationToken cancellationToken = default);
}