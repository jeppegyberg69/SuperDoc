using SuperDoc.Application.Helpers;
using SuperDoc.Shared.Helpers;
using SuperDoc.Shared.Services;

namespace SuperDoc.Application.Services;

public class NavigationService : INavigationService
{
    public async Task NavigateToAsync(string route, IDictionary<string, object>? routeParameters = null)
    {
        if (routeParameters != null)
        {
            await Shell.Current.GoToAsync(route, routeParameters);
        }

        await Shell.Current.GoToAsync(route);
    }

    public async Task NavigateToLoginPageAsync()
    {
        await Shell.Current.GoToAsync("//Login");
    }

    public async Task NavigateToMainPageAsync()
    {
        await Shell.Current.GoToAsync("//Cases");
    }

    public async Task GoBackAsync()
    {
        await Shell.Current.GoToAsync("..");
    }

    public async Task PushAsync(string page)
    {
        Type? pageType = AssemblyHelper.GetType(page);
        if (pageType != null)
        {
            ServiceHelper.EnsureServicesInitialized();
            await Shell.Current.Navigation.PushAsync((Page)ActivatorUtilities.GetServiceOrCreateInstance(ServiceHelper.Services, pageType));
        }
    }

    public async Task PopAsync()
    {
        if (Shell.Current.Navigation.NavigationStack.Count > 1)
        {
            await Shell.Current.Navigation.PopAsync();
        }
    }

    public async Task PopToRootAsync()
    {
        await Shell.Current.Navigation.PopToRootAsync();
    }
}
