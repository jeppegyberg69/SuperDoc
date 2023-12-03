namespace SuperDoc.Shared.Services.Contracts;

/// <summary>
/// Service for securely storing and retrieving sensitive data using platform-specific secure storage mechanisms.
/// </summary>
public interface ISecureStorageService
{
    /// <summary>
    /// Retrieves the value associated with the specified key from secure storage.
    /// </summary>
    /// <param name="key">The key of the value to be retrieved from secure storage.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation, containing the value associated with the specified key if the key is found; otherwise, <see langword="null"/>.
    /// </returns>
    public Task<string?> GetAsync(string key);

    /// <summary>
    /// Sets the specified value in secure storage with the given key.
    /// </summary>
    /// <param name="key">The key under which the value will be stored in secure storage.</param>
    /// <param name="value">The value to be stored securely.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public Task SetAsync(string key, string value);

    /// <summary>
    /// Removes a value associated with the specified key from the secure storage.
    /// </summary>
    /// <param name="key">The key of the value to be removed from secure storage.</param>
    /// <returns><see langword="true"/> if the value associated with the specified key is successfully removed; otherwise, <see langword="false"/>. Also, returns <see langword="false"/> if the specified key is not found.</returns>
    public bool Remove(string key);
}
