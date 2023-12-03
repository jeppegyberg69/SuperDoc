using SuperDoc.Shared.Services.Contracts;

namespace SuperDoc.Application.Services;

/// <inheritdoc cref="ISecureStorageService"/>
public class SecureStorageService : ISecureStorageService
{
    /// <inheritdoc cref="ISecureStorageService.GetAsync(string)"/>
    public async Task<string?> GetAsync(string key)
    {
        return await SecureStorage.Default.GetAsync(key);
    }

    /// <inheritdoc cref="ISecureStorageService.SetAsync(string, string)"/>
    public async Task SetAsync(string key, string value)
    {
        await SecureStorage.Default.SetAsync(key, value);
    }

    /// <inheritdoc cref="ISecureStorageService.Remove(string)"/>
    public bool Remove(string key)
    {
        return SecureStorage.Default.Remove(key);
    }
}
