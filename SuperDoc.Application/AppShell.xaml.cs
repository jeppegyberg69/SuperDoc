using System.Reflection;

namespace SuperDoc.Application;

public partial class AppShell : Shell
{
    private ShellContent? _previousShellContent;

    public AppShell()
    {
        InitializeComponent();
    }

    protected override void OnNavigated(ShellNavigatedEventArgs args)
    {
        // https://github.com/dotnet/maui/issues/9300#issuecomment-1416893792
        if (CurrentItem?.CurrentItem?.CurrentItem != null && _previousShellContent != null)
        {
            PropertyInfo? contentCacheProperty = typeof(ShellContent).GetProperty("ContentCache", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.FlattenHierarchy);
            contentCacheProperty?.SetValue(_previousShellContent, null);
        }

        _previousShellContent = CurrentItem?.CurrentItem?.CurrentItem;

        base.OnNavigated(args);
    }
}
