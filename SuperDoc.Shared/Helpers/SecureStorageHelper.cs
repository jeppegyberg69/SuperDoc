using System.Text.Json;

namespace SuperDoc.Shared.Helpers;

public static class SecureStorageHelper
{
    public const string AuthorizationTokenKey = "authentication_token";

    public static string SerializeValue(object value)
    {
        return JsonSerializer.Serialize(value);
    }

    public static TValue? DeserializeValue<TValue>(string json)
    {
        return (TValue?)JsonSerializer.Deserialize(json, typeof(TValue));
    }
}
