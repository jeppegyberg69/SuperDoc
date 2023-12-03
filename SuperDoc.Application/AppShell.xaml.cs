using System.Reflection;

using SuperDoc.Application.Views;

namespace SuperDoc.Application;

public partial class AppShell : Shell
{
    private ShellContent? _previousShellContent;

    public AppShell()
    {
        InitializeComponent();

        Routing.RegisterRoute("Cases/Details", typeof(CaseDetailsPage));
        Routing.RegisterRoute("Cases/Details/Document", typeof(DocumentPage));
    }

    protected override void OnNavigated(ShellNavigatedEventArgs args)
    {
        base.OnNavigated(args);

        // https://github.com/dotnet/maui/issues/9300#issuecomment-1416893792
        if (args.Current.Location.OriginalString == "//Cases/Details" || args.Current.Location.OriginalString == "//Cases/Details/Document")
        {
            _previousShellContent = null;
            return;
        }

        if (CurrentItem?.CurrentItem?.CurrentItem != null && _previousShellContent != null)
        {
            PropertyInfo? contentCacheProperty = typeof(ShellContent).GetProperty("ContentCache", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.FlattenHierarchy);
            contentCacheProperty?.SetValue(_previousShellContent, null);
        }

        _previousShellContent = CurrentItem?.CurrentItem?.CurrentItem;
    }
}
