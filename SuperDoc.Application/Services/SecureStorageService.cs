using SuperDoc.Shared.Services;

namespace SuperDoc.Application.Services;

public class SecureStorageService : ISecureStorageService
{
    public async Task<string?> GetAsync(string key)
    {
        return await SecureStorage.Default.GetAsync(key);
    }

    public async Task SetAsync(string key, string value)
    {
        await SecureStorage.Default.SetAsync(key, value);
    }

    public bool Remove(string key)
    {
        return SecureStorage.Default.Remove(key);
    }
}
