using System.Reflection;

using SuperDoc.Application.Views;

namespace SuperDoc.Application;

public partial class AppShell : Shell
{
    /// <summary>
    /// Represents the previous page, is used to clear page caching.
    /// </summary>
    private ShellContent? _previousShellContent;

    public AppShell()
    {
        InitializeComponent();

        // Register routes for navigating to detail pages.
        Routing.RegisterRoute("Cases/Details", typeof(CaseDetailsPage));
        Routing.RegisterRoute("Cases/Details/Document", typeof(DocumentPage));
    }

    protected override void OnNavigated(ShellNavigatedEventArgs args)
    {
        base.OnNavigated(args);

        // Temporary workaround for an issue in MAUI (https://github.com/dotnet/maui/issues/9300#issuecomment-1416893792)

        // Our fix for not caching pages when navigating completely breaks any detail pages.
        // Therefore, we keep the cache for the previous page if we are navigating to a details page.
        // Unfortunately, there is no way other than to hard code the paths; hopefully, this issue can be fixed in a later MAUI version.
        if (args.Current.Location.OriginalString == "//Cases/Details" || args.Current.Location.OriginalString == "//Cases/Details/Document")
        {
            _previousShellContent = null;
            return;
        }

        if (CurrentItem?.CurrentItem?.CurrentItem != null && _previousShellContent != null)
        {
            // Clear the content cache from the previous page when navigating to a new page.
            PropertyInfo? contentCacheProperty = typeof(ShellContent).GetProperty("ContentCache", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.FlattenHierarchy);
            contentCacheProperty?.SetValue(_previousShellContent, null);
        }

        // Update the previous ShellContent for future navigation events.
        _previousShellContent = CurrentItem?.CurrentItem?.CurrentItem;
    }
}
