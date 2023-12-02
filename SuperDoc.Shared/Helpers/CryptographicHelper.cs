using System.Security.Cryptography;

namespace SuperDoc.Shared.Helpers;

public static class CryptographicHelper
{
    private const int KeySize = 4096;

    public static RSAKeyParameters GenerateKeys()
    {
        using RSACryptoServiceProvider cryptoServiceProvider = new RSACryptoServiceProvider(KeySize);

        string publicKey = Convert.ToBase64String(cryptoServiceProvider.ExportRSAPublicKey());
        string privateKey = Convert.ToBase64String(cryptoServiceProvider.ExportRSAPrivateKey());

        return new RSAKeyParameters(publicKey, privateKey);
    }

    public static string SignData(byte[] data, string privateKey)
    {
        RSACryptoServiceProvider cryptoServiceProvider = new RSACryptoServiceProvider(KeySize);
        cryptoServiceProvider.ImportRSAPrivateKey(Convert.FromBase64String(privateKey), out _);

        byte[] signedBytes = cryptoServiceProvider.SignData(data, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);

        return BitConverter.ToString(signedBytes).Replace("-", string.Empty);
    }

    public static bool IsSignatureValid(string hash, string signature, string publicKey)
    {
        try
        {
            using RSACryptoServiceProvider cryptoServiceProvider = new RSACryptoServiceProvider(KeySize);
            cryptoServiceProvider.ImportRSAPublicKey(Convert.FromBase64String(publicKey), out _);

            return cryptoServiceProvider.VerifyHash(Convert.FromHexString(hash), Convert.FromHexString(signature), HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
        }
        catch (Exception)
        {
            return false;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="text"></param>
    /// <param name="publicKey"></param>
    /// <returns></returns>
    /// <exception cref="CryptographicException"/>
    //public static string Encrypt(string text, string publicKey)
    //{
    //    byte[] plainBytes = Encoding.UTF8.GetBytes(text);

    //    using RSACryptoServiceProvider cryptoServiceProvider = new RSACryptoServiceProvider(KeySize) { PersistKeyInCsp = false };

    //    cryptoServiceProvider.ImportFromPem(publicKey);
    //    byte[] encryptedBytes = cryptoServiceProvider.Encrypt(plainBytes, true);

    //    return Convert.ToBase64String(encryptedBytes);
    //}

    public static string HashData(byte[] data)
    {
        return BitConverter.ToString(SHA256.HashData(data)).Replace("-", string.Empty);
    }

    public static async Task<string> HashDocumentAsync(Stream documentStream)
    {
        using SHA256 sha256 = SHA256.Create();

        documentStream.Position = 0;

        byte[] hashedBytes = await sha256.ComputeHashAsync(documentStream);
        return BitConverter.ToString(hashedBytes).Replace("-", string.Empty);
    }
}

public struct RSAKeyParameters(string publicKey, string privateKey)
{
    public string PublicKey { get; private set; } = publicKey;

    public string PrivateKey { get; private set; } = privateKey;
}
