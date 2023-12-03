using System.Net;
using System.Net.Http.Json;

using SuperDoc.Shared.Exceptions;
using SuperDoc.Shared.Models.Documents;
using SuperDoc.Shared.Models.Revisions;
using SuperDoc.Shared.Services.Contracts;

namespace SuperDoc.Shared.Services;

/// <inheritdoc cref="IDocumentService"/>
public class DocumentService(IHttpClientFactory httpClientFactory) : BaseHttpService("CustomerAPI", string.Empty, httpClientFactory), IDocumentService
{
    /// <inheritdoc cref="IDocumentService.GetDocumentRevisionsAsync(Guid, CancellationToken)"/>
    public async Task<IEnumerable<RevisionDto>> GetDocumentRevisionsAsync(Guid documentId, CancellationToken cancellationToken = default)
    {
        HttpResponseMessage? response = default;
        try
        {
            // Send an asynchronous HTTP GET request to retrieve revisions by documentId.
            response = await HttpClient.GetAsync($"Revision/GetRevisionsByDocumentId?documentId={documentId}", cancellationToken);
        }
        catch (Exception exception)
        {
            // An unexpected exception occurred during the HTTP request; throw an unexpected HttpServiceException.
            HttpServiceException.ThrowInvalidStatusCode(response?.Content, response?.StatusCode, exception);
        }

        // If the HTTP response status code doesn't indicate success then throw an invalid status code HttpServiceException.
        if (!response.IsSuccessStatusCode)
        {
            HttpServiceException.ThrowInvalidStatusCode(response?.Content, response?.StatusCode);
        }

        // Read and deserialize the response content into a collection of RevisionDto.
        IEnumerable<RevisionDto>? revisions = await response.Content.ReadFromJsonAsync<IEnumerable<RevisionDto>?>(cancellationToken);

        return revisions ?? Enumerable.Empty<RevisionDto>();
    }

    /// <inheritdoc cref="IDocumentService.GetDocumentStreamAsync(Guid, CancellationToken)"/>
    public async Task<Stream?> GetDocumentStreamAsync(Guid revisionId, CancellationToken cancellationToken = default)
    {
        HttpResponseMessage? response = default;
        try
        {
            // Send an asynchronous HTTP GET request to download the revision by revisionId.
            response = await HttpClient.GetAsync($"Revision/DownloadRevision?revisionId={revisionId}", cancellationToken);
        }
        catch (Exception exception)
        {
            // An unexpected exception occurred during the HTTP request; throw an unexpected HttpServiceException.
            HttpServiceException.ThrowInvalidStatusCode(response?.Content, response?.StatusCode, exception);
        }

        // If the response status code is 404 (Not Found), then no document is found with the specified revision id, so return null.
        // Otherwise if the status code is not successful, throw an invalid status code HttpServiceException.
        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            return null;
        }
        else if (!response.IsSuccessStatusCode)
        {
            HttpServiceException.ThrowInvalidStatusCode(response?.Content, response?.StatusCode);
        }

        // Read the response content as a stream.
        Stream? documentStream = await response.Content.ReadAsStreamAsync(cancellationToken);

        return documentStream;
    }

    /// <inheritdoc cref="IDocumentService.SignDocumentRevisionAsync(Guid, string, string)"/>
    public async Task<DocumentSignatureDto?> SignDocumentRevisionAsync(Guid revisionId, string signature, string publicKey, CancellationToken cancellationToken = default)
    {
        HttpResponseMessage? response = default;
        try
        {
            // Send an asynchronous HTTP POST request to sign the document revision.
            response = await HttpClient.PostAsJsonAsync($"Revision/SignRevision", new { RevisionId = revisionId, PublicKey = publicKey, Signature = signature }, cancellationToken);
        }
        catch (Exception exception)
        {
            // An unexpected exception occurred during the HTTP request; throw an unexpected HttpServiceException.
            HttpServiceException.ThrowInvalidStatusCode(response?.Content, response?.StatusCode, exception);
        }

        // If the HTTP response status code doesn't indicate success then throw an invalid status code HttpServiceException.
        if (!response.IsSuccessStatusCode)
        {
            HttpServiceException.ThrowInvalidStatusCode(response.Content, response.StatusCode);
        }

        // Read and deserialize the response content into a DocumentSignatureDto.
        DocumentSignatureDto? documentSignature = await response.Content.ReadFromJsonAsync<DocumentSignatureDto>(cancellationToken);

        return documentSignature;
    }
}
