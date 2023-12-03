using System.Diagnostics.CodeAnalysis;

namespace SuperDoc.Application.Helpers;

/// <summary>
/// Helper class for managing and retrieving services within the application.
/// </summary>
public static class ServiceHelper
{
    /// <summary>
    /// Gets or sets the service provider used for service retrieval.
    /// </summary>
    public static IServiceProvider? Services { get; private set; }

    /// <summary>
    /// Initializes the <see cref="ServiceHelper"/> with the provided service provider.
    /// </summary>
    /// <param name="serviceProvider">The service provider to be used for service retrieval.</param>
    public static void Initialize(IServiceProvider serviceProvider)
    {
        Services = serviceProvider;
    }

    /// <summary>
    /// Retrieves a service of type <typeparamref name="TService"/> from the service provider.
    /// </summary>
    /// <typeparam name="TService">The type of service to retrieve.</typeparam>
    /// <returns>The retrieved service or <see langword="null"/> if the service is not found.</returns>
    public static TService? GetService<TService>()
    {
        // Ensure that ServiceHelper is initialized before attempting to retrieve the service.
        EnsureServicesInitialized();

        return Services.GetService<TService>();
    }

    /// <summary>
    /// Throws an <see cref="InvalidOperationException"/> if <see cref="Services"/> is null.
    /// </summary>
    [MemberNotNull(nameof(Services))] // Tell the compiler that Services will be not-null after the method has executed.
    internal static void EnsureServicesInitialized()
    {
        if (Services == null)
        {
            throw new InvalidOperationException("Services cannot be null. Initialize the services before calling this method.");
        }
    }
}
