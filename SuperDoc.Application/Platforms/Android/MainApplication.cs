using Android.App;
using Android.Runtime;

namespace SuperDoc.Application;

// Allow cleartext traffic, so that our Proof of Concept application can communicate with the API using HTTP, as we don't have a SSL certificate yet.
[Application(UsesCleartextTraffic = true)]
public class MainApplication : MauiApplication
{
    public MainApplication(IntPtr handle, JniHandleOwnership ownership) : base(handle, ownership)
    {

    }

    protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
}
