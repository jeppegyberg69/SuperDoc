using Microsoft.Extensions.Configuration;

using SuperDoc.Application.Helpers;

using Syncfusion.Licensing;

namespace SuperDoc.Application;

public partial class App : Microsoft.Maui.Controls.Application
{
    public App()
    {
        // Get the IConfiguration service using ServiceHelper, so that we can retrieve the Syncfusion license and register it.
        IConfiguration? configuration = ServiceHelper.GetService<IConfiguration>();
        if (configuration != null)
        {
            SyncfusionLicenseProvider.RegisterLicense(configuration["Syncfusion:License"]);
        }

        InitializeComponent();

        MainPage = new AppShell();
    }

    /// <summary>
    /// Called when the application starts.
    /// </summary>
    protected override void OnStart()
    {
        base.OnStart();
    }

    /// <summary>
    /// Called when the application is resumed after being in the background.
    /// </summary>
    protected override void OnResume()
    {
        base.OnResume();
    }

    /// <summary>
    /// Called when the application is sent to the background or goes to sleep.
    /// </summary>
    protected override void OnSleep()
    {
        base.OnSleep();
    }
}
