using System.Security.Cryptography;

namespace SuperDoc.Shared.Helpers;

/// <summary>
/// Helper class for cryptographic operations.
/// </summary>
public static class CryptographicHelper
{
    /// <summary>
    /// Specifies the key size used for RSA key generation.
    /// </summary>
    public const int KeySize = 4096;

    /// <summary>
    /// Generates a pair of RSA public and private keys.
    /// </summary>
    /// <returns>An instance of <see cref="RSAKeys"/> containing the generated public and private keys.</returns>
    public static RSAKeys GenerateKeys()
    {
        using RSACryptoServiceProvider cryptoServiceProvider = new RSACryptoServiceProvider(KeySize);

        // Export the generated public key to Base64 string.
        string publicKey = Convert.ToBase64String(cryptoServiceProvider.ExportRSAPublicKey());

        // Export the generated private key to Base64 string.
        string privateKey = Convert.ToBase64String(cryptoServiceProvider.ExportRSAPrivateKey());

        // Return an instance of RSAKeys containing the generated public and private keys.
        return new RSAKeys(publicKey, privateKey);
    }

    /// <summary>
    /// Signs the given data using the specified private key.
    /// </summary>
    /// <param name="data">The data to be signed.</param>
    /// <param name="privateKey">The private key used for signing.</param>
    /// <returns>A hexadecimal <see cref="string"/> representing the signature of the data.</returns
    public static string SignData(byte[] data, string privateKey)
    {
        RSACryptoServiceProvider cryptoServiceProvider = new RSACryptoServiceProvider(KeySize);

        // Import the private key into the cryptoServiceProvider.
        cryptoServiceProvider.ImportRSAPrivateKey(Convert.FromBase64String(privateKey), out _);

        // Sign the data using SHA-256 hash algorithm and Pkcs1 padding.
        byte[] signedBytes = cryptoServiceProvider.SignData(data, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);

        // Convert the signature bytes to a hexadecimal string.
        return BitConverter.ToString(signedBytes).Replace("-", string.Empty);
    }

    /// <summary>
    /// Verifies if a given signature is valid for a specified hash and public key.
    /// </summary>
    /// <param name="hash">The hash value to be verified.</param>
    /// <param name="signature">The signature to be validated.</param>
    /// <param name="publicKey">The public key used for signature verification.</param>
    /// <returns><see langword="true"/> if the signature is valid; otherwise, <see langword="false"/>.</returns>
    public static bool IsSignatureValid(string hash, string signature, string publicKey)
    {
        try
        {
            using RSACryptoServiceProvider cryptoServiceProvider = new RSACryptoServiceProvider(KeySize);

            // Import the public key into the cryptoServiceProvider.
            cryptoServiceProvider.ImportRSAPublicKey(Convert.FromBase64String(publicKey), out _);

            // Verify the signature using SHA-256 hash algorithm and Pkcs1 padding.
            return cryptoServiceProvider.VerifyHash(Convert.FromHexString(hash), Convert.FromHexString(signature), HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
        }
        catch (Exception)
        {
            // If an exception occurs during the verification process, return false.
            return false;
        }
    }

    /// <summary>
    /// Computes the SHA-256 hash of a stream asynchronously.
    /// </summary>
    /// <param name="stream">The stream containing the data.</param>
    /// <returns>A hexadecimal <see cref="string"/> representing the SHA-256 hash of the stream.</returns>
    public static async Task<string> HashStreamAsync(Stream stream)
    {
        using SHA256 sha256 = SHA256.Create();

        // Ensure that the stream is at the beginning, so that we compute the stream correctly.
        stream.Position = 0;

        // Compute the hash asynchronously and convert the result to a hexadecimal string.
        byte[] hashedBytes = await sha256.ComputeHashAsync(stream);
        return BitConverter.ToString(hashedBytes).Replace("-", string.Empty);
    }
}


/// <summary>
/// Represents a readonly structure containing RSA keys.
/// </summary>
/// <param name="publicKey">The public key string.</param>
/// <param name="privateKey">The private key string.</param>
public readonly struct RSAKeys(string publicKey, string privateKey)
{
    /// <summary>
    /// Gets the public key string.
    /// </summary>
    public string PublicKey { get; init; } = publicKey;

    /// <summary>
    /// Gets the private key string.
    /// </summary>
    public string PrivateKey { get; init; } = privateKey;
}
