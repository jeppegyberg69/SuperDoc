using Android.App;
using Android.Content.PM;
using Android.OS;

namespace SuperDoc.Application;
[Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
public class MainActivity : MauiAppCompatActivity
{
    protected override void OnCreate(Bundle? savedInstanceState)
    {
        // Set the correct color for the navigation bar.
        Window?.SetNavigationBarColor(Android.Graphics.Color.Argb(255, 235, 238, 232));

        base.OnCreate(savedInstanceState);
    }
}
