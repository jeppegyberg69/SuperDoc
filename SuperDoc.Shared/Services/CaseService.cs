using System.Net.Http.Json;

using SuperDoc.Shared.Exceptions;
using SuperDoc.Shared.Models.Cases;
using SuperDoc.Shared.Models.Documents;
using SuperDoc.Shared.Services.Contracts;

namespace SuperDoc.Shared.Services;

/// <inheritdoc cref="ICaseService"/>
public class CaseService(IHttpClientFactory httpClientFactory) : BaseHttpService("CustomerAPI", string.Empty, httpClientFactory), ICaseService
{
    /// <inheritdoc cref="ICaseService.GetCasesAsync(CancellationToken)"/>
    public async Task<IEnumerable<CaseDto>> GetCasesAsync(CancellationToken cancellationToken = default)
    {
        HttpResponseMessage? response = default;
        try
        {
            // Send an asynchronous HTTP GET request to retrieve cases.
            response = await HttpClient.GetAsync("Case/GetCases", cancellationToken);
        }
        catch (Exception exception)
        {
            // An unexpected exception occurred during the HTTP request; throw an unexpected HttpServiceException.
            HttpServiceException.ThrowUnexpectedError(response?.Content, response?.StatusCode, exception);
        }

        // If the HTTP response status code doesn't indicate success then throw an invalid status code HttpServiceException.
        if (!response.IsSuccessStatusCode)
        {
            HttpServiceException.ThrowInvalidStatusCode(response?.Content, response?.StatusCode);
        }

        // Read and deserialize the response content into a collection of CaseDto.
        IEnumerable<CaseDto>? cases = await response.Content.ReadFromJsonAsync<IEnumerable<CaseDto>?>(cancellationToken);

        return cases ?? Enumerable.Empty<CaseDto>();
    }

    /// <inheritdoc cref="ICaseService.GetCaseDocumentsAsync(Guid, CancellationToken)"/>
    public async Task<IEnumerable<DocumentDto>> GetCaseDocumentsAsync(Guid caseId, CancellationToken cancellationToken = default)
    {
        HttpResponseMessage? response = default;
        try
        {
            // Send an asynchronous HTTP GET request to retrieve documents.
            response = await HttpClient.GetAsync($"Document/GetDocumentsByCaseId?caseId={caseId}", cancellationToken);
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

        // Read and deserialize the response content into a collection of DocumentDto.
        IEnumerable<DocumentDto>? documents = await response.Content.ReadFromJsonAsync<IEnumerable<DocumentDto>?>(cancellationToken);

        return documents ?? Enumerable.Empty<DocumentDto>();
    }
}
