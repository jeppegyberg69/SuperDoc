using System.Net;
using System.Net.Http.Json;

using SuperDoc.Shared.Exceptions;
using SuperDoc.Shared.Helpers;
using SuperDoc.Shared.Models.Users;

namespace SuperDoc.Shared.Services;

/// <summary>
/// Manages user authentication, including login and token retrieval, and related operations.
/// </summary>
/// <param name="secureStorageService">An implementation of the <see cref="ISecureStorageService"/> interface for secure storage operations.</param>
/// <param name="httpClientFactory">An implementation of the <see cref="IHttpClientFactory"/> interface for creating and managing HttpClient instances.</param>
public class AuthenticationService(ISecureStorageService secureStorageService, IHttpClientFactory httpClientFactory) : BaseHttpService("SuperDoc", "user/", httpClientFactory)
{
    /// <summary>
    /// Checks if the user is authenticated by validating the stored authentication token.
    /// </summary>
    /// <returns><see langword="true"/>  if the user is authenticated; otherwise, returns <see langword="false"/>.</returns>
    public async Task<bool> CheckAuthentication()
    {
        // Retrieve the authentication token asynchronously.
        TokenDto? token = await GetTokenAsync();
        if (token == null)
        {
            return false;
        }

        // Validate the retrieved token and revoke it if invalid.
        bool isAuthenticated = ValidateToken(token);
        if (!isAuthenticated)
        {
            RevokeToken();
        }

        return isAuthenticated;
    }

    /// <summary>
    /// Authenticates a user by sending a login request with the provided email and password.
    /// </summary>
    /// <param name="email">The user's email address.</param>
    /// <param name="password">The user's password.</param>
    /// <param name="cancellationToken">Optional cancellation token to cancel the asynchronous operation.</param>
    /// <returns>The authentication token if successful; otherwise, returns <see langword="null"/>.</returns>
    /// <exception cref="HttpServiceException">Thrown when an unexpected error occurs during the HTTP request or if the response indicates failure.</exception>
    public async Task<TokenDto?> LoginAsync(string email, string password, CancellationToken cancellationToken = default)
    {
        HttpResponseMessage? response = default;
        try
        {
            // Send a POST request to the "login" endpoint with the provided email and password.
            response = await HttpClient.PostAsJsonAsync("login", new { Email = email, Password = password }, cancellationToken);
        }
        catch (Exception exception)
        {
            // An unexpected exception occurred during the HTTP request; log the error and throw an HttpServiceException.
            throw new HttpServiceException("Unexpected error occurred during HTTP request.", response?.Content, response?.StatusCode, exception);
        }

        // If the response status code is 404 (Not Found), then no user is found with the specified email and password combination, so return null.
        // Otherwise if the status code is not successful, throw an HttpServiceException.
        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            return null;
        }
        else if (!response.IsSuccessStatusCode)
        {
            throw new HttpServiceException("HTTP response was not successful.", response.Content, response.StatusCode);
        }

        // Read the authentication token from the response content.
        TokenDto? token = await response.Content.ReadFromJsonAsync<TokenDto?>(cancellationToken);

        // If a token is successfully retrieved then securely store the token.
        if (token != null)
        {
            await secureStorageService.SetAsync(SecureStorageHelper.AuthorizationTokenKey, SecureStorageHelper.SerializeValue(token));
        }

        return token;
    }

    /// <summary>
    /// Retrieves the authentication token asynchronously from secure storage.
    /// </summary>
    /// <returns>The authentication token if found; otherwise, returns <see langword="null"/>.</returns>
    public async Task<TokenDto?> GetTokenAsync()
    {
        // Retrieve the serialized token from secure storage based on the AuthorizationTokenKey.
        string? serializedToken = await secureStorageService.GetAsync(SecureStorageHelper.AuthorizationTokenKey);
        if (serializedToken == null)
        {
            return null;
        }

        // Deserialize the serialized token and return the resulting authentication token.
        return SecureStorageHelper.DeserializeValue<TokenDto>(serializedToken);
    }

    /// <summary>
    /// Validates the provided authentication token.
    /// </summary>
    /// <param name="token">The authentication token to be validated.</param>
    /// <returns><see langword="true"/> if the token is valid; otherwise, returns <see langword="false"/>.</returns>
    public static bool ValidateToken(TokenDto token)
    {
        // Check if the current UTC time is within the token's validity period.
        bool isValid = DateTime.UtcNow > token.ValidFrom && DateTime.UtcNow < token.ValidTo;

        return isValid;
    }

    /// <summary>
    /// Revokes the stored authentication token by removing it from secure storage.
    /// </summary>
    /// <returns><see langword="true"/> if the token revocation is successful; otherwise, returns <see langword="false"/>.</returns>
    public bool RevokeToken()
    {
        // Remove the authentication token from secure storage based on the AuthorizationTokenKey.
        return secureStorageService.Remove(SecureStorageHelper.AuthorizationTokenKey);
    }
}
