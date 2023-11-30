using System.Diagnostics.CodeAnalysis;

namespace SuperDoc.Application.Helpers;

public static class ServiceHelper
{
    public static IServiceProvider? Services { get; private set; }

    public static void Initialize(IServiceProvider serviceProvider)
    {
        Services = serviceProvider;
    }

    public static TService? GetService<TService>()
    {
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
