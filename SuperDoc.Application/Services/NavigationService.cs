using SuperDoc.Shared.Services.Contracts;

namespace SuperDoc.Application.Services;

/// <inheritdoc cref="INavigationService"/>
public class NavigationService : INavigationService
{
    /// <inheritdoc cref="INavigationService.NavigateToPageAsync(string, IDictionary{string, object}?)"/>
    public async Task NavigateToPageAsync(string route, IDictionary<string, object>? parameters = null)
    {
        if (parameters != null)
        {
            await Shell.Current.GoToAsync(route, parameters);
            return;
        }

        await Shell.Current.GoToAsync(route);
    }

    /// <inheritdoc cref="INavigationService.NavigateToLoginPageAsync"/>
    public async Task NavigateToLoginPageAsync()
    {
        await Shell.Current.GoToAsync("//Login");
    }

    /// <inheritdoc cref="INavigationService.NavigateToMainPageAsync"/>
    public async Task NavigateToMainPageAsync()
    {
        await Shell.Current.GoToAsync("//Cases");
    }

    /// <inheritdoc cref="INavigationService.GoBackAsync"/>
    public async Task GoBackAsync()
    {
        await Shell.Current.GoToAsync("..");
    }
}
