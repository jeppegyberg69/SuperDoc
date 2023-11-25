using CommunityToolkit.Maui;

using Microsoft.Extensions.Logging;

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

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}
