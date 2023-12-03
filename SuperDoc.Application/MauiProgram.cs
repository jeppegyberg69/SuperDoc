using System.Reflection;

using CommunityToolkit.Maui;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

using SuperDoc.Application.Helpers;
using SuperDoc.Application.Services;
using SuperDoc.Shared.Services;
using SuperDoc.Shared.Services.Contracts;
using Syncfusion.Maui.Core.Hosting;

namespace SuperDoc.Application;

public static class MauiProgram
{
    /// <summary>
    /// Creates and configures a .NET MAUI application.
    /// </summary>
    public static MauiApp CreateMauiApp()
    {
        MauiAppBuilder builder = MauiApp.CreateBuilder();

        builder.UseMauiApp<App>().ConfigureFonts(fonts =>
        {
            fonts.AddFont("Inter-Regular.ttf", "InterRegular");
            fonts.AddFont("Inter-SemiBold.ttf", "InterSemiBold");
            fonts.AddFont("Inter-Bold.ttf", "InterBold");
            fonts.AddFont("Inter-ExtraBold.ttf", "InterExtraBold");

            fonts.AddFont("FluentSystemIcons-Regular.ttf", "FluentSystemIconsRegular");
            fonts.AddFont("FluentSystemIcons-Filled.ttf", "FluentSystemIconsFilled");
        });

        // Initialize the .NET MAUI Community Toolkit.
        builder.UseMauiCommunityToolkit();

        // Initialize Syncfusion.
        builder.ConfigureSyncfusionCore();

        builder.ConfigureConfiguration();
        builder.ConfigureServices(builder.Configuration);

#if DEBUG
        builder.Logging.AddDebug();
#endif

        // Build and initialize the MauiApp instance.
        MauiApp app = builder.Build();

        // Initialize ServiceHelper, so that we can use the ViewModelLocator.
        ServiceHelper.Initialize(app.Services);

        return app;
    }

    /// <summary>
    /// Configures application settings by loading them from an embedded JSON resource file.
    /// </summary>
    /// <param name="builder">The <see cref="MauiAppBuilder"/> instance to configure.</param>
    /// <exception cref="InvalidOperationException">Thrown if the appsettings.json resource is not found.</exception>
    private static void ConfigureConfiguration(this MauiAppBuilder builder)
    {
        // Get the stream for the embedded appsettings.json resource.
        using Stream? stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("SuperDoc.Application.appsettings.json") ?? throw new InvalidOperationException("Unable to locate the appsettings.json resource. Make sure the resource file is included in the project and has the correct name.");

        IConfiguration configuration = new ConfigurationBuilder().AddJsonStream(stream).Build();
        builder.Configuration.AddConfiguration(configuration);
    }

    /// <summary>
    /// Configures and registers services for the application using the provided configuration.
    /// </summary>
    /// <param name="builder">The <see cref="MauiAppBuilder"/> instance to configure.</param>
    /// <param name="configuration">The application configuration.</param>
    /// <exception cref="InvalidOperationException">Thrown if required configuration values are missing or null.</exception>
    private static void ConfigureServices(this MauiAppBuilder builder, IConfiguration configuration)
    {
        // Registering application services.
        builder.Services.AddSingleton<IAuthenticationService, AuthenticationService>();
        builder.Services.AddSingleton<ICaseService, CaseService>();
        builder.Services.AddSingleton<IDocumentService, DocumentService>();

        builder.Services.AddSingleton<IDialogService, DialogService>();
        builder.Services.AddSingleton<ISecureStorageService, SecureStorageService>();
        builder.Services.AddSingleton<INavigationService, NavigationService>();

        // Configuring and registering HttpClient for the CustomerAPI.
        builder.Services.AddHttpClient("CustomerAPI", client =>
        {
            string? customerBaseAddress = configuration["CustomerAPI:BaseAddress"];
            if (string.IsNullOrWhiteSpace(customerBaseAddress))
            {
                throw new InvalidOperationException("CustomerAPI BaseAddress is missing or null. Please check the configuration.");
            }

            client.BaseAddress = new Uri(customerBaseAddress);
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            client.Timeout = TimeSpan.FromSeconds(10);
        });

        // Configuring and registering HttpClient for the TrustedEntityAPI.
        builder.Services.AddHttpClient("TrustedEntityAPI", client =>
        {
            string? trustedEntityBaseAddress = configuration["TrustedEntityAPI:BaseAddress"];
            if (string.IsNullOrWhiteSpace(trustedEntityBaseAddress))
            {
                throw new InvalidOperationException("TrustedEntityAPI BaseAddress is missing or null. Please check the configuration.");
            }

            client.BaseAddress = new Uri(trustedEntityBaseAddress);
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            client.Timeout = TimeSpan.FromSeconds(10);
        });
    }
}
