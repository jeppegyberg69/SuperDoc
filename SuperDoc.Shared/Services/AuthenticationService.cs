using System.Net;
using System.Net.Http.Json;

using SuperDoc.Shared.Exceptions;
using SuperDoc.Shared.Helpers;
using SuperDoc.Shared.Models.Users;

namespace SuperDoc.Shared.Services;

/// <inheritdoc cref="IAuthenticationService"/>
public class AuthenticationService(ISecureStorageService secureStorageService, IHttpClientFactory httpClientFactory) : BaseHttpService("CustomerAPI", "user/", httpClientFactory), IAuthenticationService
{
    /// <inheritdoc cref="IAuthenticationService.CheckAuthenticationAsync"/>
    public async Task<bool> CheckAuthenticationAsync()
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

    /// <inheritdoc cref="IAuthenticationService.LoginAsync(string, string, CancellationToken)"/>
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

    /// <inheritdoc cref="IAuthenticationService.GetTokenAsync"/>
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

    /// <inheritdoc cref="IAuthenticationService.RevokeToken"/>
    public bool RevokeToken()
    {
        // Remove the authentication token from secure storage based on the AuthorizationTokenKey.
        return secureStorageService.Remove(SecureStorageHelper.AuthorizationTokenKey);
    }
}
