using SuperDoc.Shared.Models.Users;

namespace SuperDoc.Shared.Services.Contracts;

/// <summary>
/// Service for managing user authentication, including login and token retrieval, and related operations.
/// </summary>
/// <param name="secureStorageService">An implementation of the <see cref="ISecureStorageService"/> interface for secure storage operations.</param>
/// <param name="httpClientFactory">The factory for creating an instances of <see cref="HttpClient"/>.</param>
public interface IAuthenticationService : IHttpService
{
    /// <summary>
    /// Checks if the user is authenticated by validating the stored authentication token.
    /// </summary>
    /// <returns><see langword="true"/> if the user is authenticated; otherwise, returns <see langword="false"/>.</returns>
    public Task<bool> CheckAuthenticationAsync();

    /// <summary>
    /// Authenticates a user by sending a login request with the provided email and password.
    /// </summary>
    /// <param name="email">The user's email address.</param>
    /// <param name="password">The user's password.</param>
    /// <param name="cancellationToken">Optional cancellation token to cancel the asynchronous operation.</param>
    /// <returns>The authentication token if successful; otherwise, returns <see langword="null"/>.</returns>
    /// <exception cref="HttpServiceException">Thrown when an unexpected error occurs during the HTTP request or if the response indicates failure.</exception>
    public Task<TokenDto?> LoginAsync(string email, string password, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves the authentication token asynchronously from secure storage.
    /// </summary>
    /// <returns>The authentication token if found; otherwise, returns <see langword="null"/>.</returns>
    public Task<TokenDto?> GetTokenAsync();

    /// <summary>
    /// Revokes the stored authentication token by removing it from secure storage.
    /// </summary>
    /// <returns><see langword="true"/> if the token revocation is successful; otherwise, returns <see langword="false"/>.</returns>
    public bool RevokeToken();
}