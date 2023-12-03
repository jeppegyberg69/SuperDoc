namespace SuperDoc.Shared.Services.Contracts;

/// <summary>
/// Provides a base abstract class for HTTP services, encapsulating common functionality for making HTTP requests.
/// </summary>
public interface IHttpService
{
    /// <summary>
    /// Sets the authorization token for HTTP requests using the Bearer authentication scheme.
    /// </summary>
    /// <param name="token">The authorization token to be set.</param>
    public void SetAuthorization(string token);
}