using System.Reflection;

using CommunityToolkit.Maui;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

using SuperDoc.Application.Helpers;
using SuperDoc.Application.Services;
using SuperDoc.Shared.Services;

namespace SuperDoc.Application;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        MauiAppBuilder builder = MauiApp.CreateBuilder();

        builder.UseMauiApp<App>()
            .UseMauiCommunityToolkit() // Initialize the .NET MAUI Community Toolki
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("Inter-Regular.ttf", "InterRegular");
                fonts.AddFont("Inter-SemiBold.ttf", "InterSemiBold");
                fonts.AddFont("Inter-Bold.ttf", "InterBold");
                fonts.AddFont("Inter-ExtraBold.ttf", "InterExtraBold");

                fonts.AddFont("FluentSystemIcons-Regular.ttf", "FluentSystemIconsRegular");
                fonts.AddFont("FluentSystemIcons-Filled.ttf", "FluentSystemIconsFilled");
            });

        builder.ConfigureConfiguration();
        builder.ConfigureServices(builder.Configuration);

#if DEBUG
        builder.Logging.AddDebug();
#endif

        MauiApp app = builder.Build();

        ServiceHelper.Initialize(app.Services);

        return app;
    }

    private static void ConfigureConfiguration(this MauiAppBuilder builder)
    {
        using Stream? stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("SuperDoc.Application.appsettings.json") ?? throw new InvalidOperationException("Unable to locate the appsettings.json resource. Make sure the resource file is included in the project and has the correct name.");

        IConfiguration configuration = new ConfigurationBuilder().AddJsonStream(stream).Build();
        builder.Configuration.AddConfiguration(configuration);
    }

    private static void ConfigureServices(this MauiAppBuilder builder, IConfiguration configuration)
    {
        builder.Services.AddSingleton<IAuthenticationService, AuthenticationService>();

        builder.Services.AddSingleton<ISecureStorageService, SecureStorageService>();
        builder.Services.AddSingleton<INavigationService, NavigationService>();

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
