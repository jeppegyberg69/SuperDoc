using System.Text.Json;

namespace SuperDoc.Shared.Helpers;

/// <summary>
/// Helper class containing constant keys and methods for secure storage operations.
/// </summary>
public static class SecureStorageHelper
{
    /// <summary>
    /// Key for storing and retrieving the authentication token in secure storage.
    /// </summary>
    public const string AuthorizationTokenKey = "authentication_token";

    /// <summary>
    /// Suffix for storing and retrieving the authorized user's public key in secure storage by appending to their email.
    /// </summary>
    public const string PublicKeySufffix = "_public_key";

    /// <summary>
    /// Suffix for storing and retrieving the authorized user's private key in secure storage by appending to their email.
    /// </summary>
    public const string PrivateKeySuffix = "_private_key";

    /// <summary>
    /// Serialize an object to a JSON string for storaging in secure storage.
    /// </summary>
    /// <param name="value">The object to be serialized.</param>
    /// <returns>The JSON string representing the serialized object.</returns>
    public static string SerializeValue(object value)
    {
        return JsonSerializer.Serialize(value);
    }

    /// <summary>
    /// Deserializes a stored JSON string into an object of the specified type.
    /// </summary>
    /// <typeparam name="TValue">The type of the object to deserialize into.</typeparam>
    /// <param name="json">The JSON string to deserialize.</param>
    /// <returns>An object of the specified type or <see langword="null"/> if deserialization fails.</returns>
    public static TValue? DeserializeValue<TValue>(string json)
    {
        TValue? value;
        try
        {
            value = (TValue?)JsonSerializer.Deserialize(json, typeof(TValue));
        }
        catch (Exception)
        {
            return default;
        }

        return value;
    }
}
