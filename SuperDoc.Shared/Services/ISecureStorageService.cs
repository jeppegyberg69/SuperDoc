namespace SuperDoc.Shared.Services;

public interface ISecureStorageService
{
    public Task<string?> GetAsync(string key);

    public Task SetAsync(string key, string value);

    public bool Remove(string key);
}
