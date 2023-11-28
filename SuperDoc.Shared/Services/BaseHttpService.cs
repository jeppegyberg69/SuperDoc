namespace SuperDoc.Shared.Services;

public abstract class BaseHttpService
{
    public BaseHttpService(string client, string address, IHttpClientFactory httpClientFactory)
    {
        HttpClient = httpClientFactory.CreateClient(client);

        HttpClient.BaseAddress = new Uri($"{HttpClient.BaseAddress}{address}");
    }

    protected HttpClient HttpClient { get; private set; }
}
