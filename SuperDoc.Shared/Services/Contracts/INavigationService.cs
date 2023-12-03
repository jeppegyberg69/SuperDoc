namespace SuperDoc.Shared.Services.Contracts;

/// <summary>
/// Service for managing navigation within the application.
/// </summary>
public interface INavigationService
{
    /// <summary>
    /// Navigates to a page with the specified route and parameters.
    /// </summary>
    /// <param name="route">The route to navigate to.</param>
    /// <param name="parameters">Optional parameters to pass during navigation.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public Task NavigateToPageAsync(string route, IDictionary<string, object>? routeParameters = null);

    /// <summary>
    /// Navigates to the login page.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public Task NavigateToMainPageAsync();

    /// <summary>
    /// Navigates to the main page.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public Task NavigateToLoginPageAsync();

    /// <summary>
    /// Navigates back in the navigation stack.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public Task GoBackAsync();
}
