namespace SuperDoc.Shared.Services;

public interface INavigationService
{
    public Task NavigateToAsync(string route, IDictionary<string, object>? routeParameters = null);

    public Task NavigateToMainPageAsync();

    public Task NavigateToLoginPageAsync();

    public Task GoBackAsync();

    public Task PushAsync(string route);

    public Task PopAsync();

    public Task PopToRootAsync();
}
