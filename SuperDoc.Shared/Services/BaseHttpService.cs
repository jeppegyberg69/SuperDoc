using System.Net.Http.Headers;
using SuperDoc.Shared.Services.Contracts;

namespace SuperDoc.Shared.Services;

/// <inheritdoc cref="IHttpService"/>
public abstract class BaseHttpService : IHttpService
{
    /// <summary>
    /// Initializes a new instance of the <see cref="BaseHttpService"/> class with the specified HttpClient, base address, and HttpClientFactory.
    /// </summary>
    /// <param name="client">The name of the <see cref="System.Net.Http.HttpClient"/> used for making HTTP requests.</param>
    /// <param name="address">The base address appended to the HttpClient's base address.</param>
    /// <param name="httpClientFactory">The factory for creating HttpClient instances.</param>
    public BaseHttpService(string client, string address, IHttpClientFactory httpClientFactory)
    {
        HttpClient = httpClientFactory.CreateClient(client);

        // Set the base address for the HttpClient by appending the provided address.
        HttpClient.BaseAddress = new Uri($"{HttpClient.BaseAddress}{address}");
    }

    /// <summary>
    /// Gets the <see cref="System.Net.Http.HttpClient"/> instance used for making HTTP requests.
    /// </summary>
    protected HttpClient HttpClient { get; init; }

    /// <inheritdoc cref="IHttpService.SetAuthorization(string)"/>
    public void SetAuthorization(string token)
    {
        // Set the authorization header with the provided token using the Bearer authentication scheme.
        HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }
}
