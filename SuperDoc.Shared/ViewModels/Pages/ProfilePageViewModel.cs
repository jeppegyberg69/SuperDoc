using CommunityToolkit.Mvvm.Input;

using SuperDoc.Shared.Models.Users;
using SuperDoc.Shared.Services;
using SuperDoc.Shared.Services.Contracts;
using SuperDoc.Shared.ViewModels.Wrappers;

namespace SuperDoc.Shared.ViewModels.Pages;

public partial class ProfilePageViewModel : BaseViewModel
{
    private readonly IAuthenticationService _authenticationService;
    private readonly INavigationService _navigationService;
    private readonly IDialogService _dialogService;

    public ProfilePageViewModel(IAuthenticationService authenticationService, INavigationService navigationService, IDialogService dialogService)
    {
        _authenticationService = authenticationService;
        _navigationService = navigationService;
        _dialogService = dialogService;

        Initialization = InitializeAsync();
    }

    private UserViewModel _authenticatedUser = null!;
    public UserViewModel AuthenticatedUser
    {
        get => _authenticatedUser;
        private set => SetProperty(ref _authenticatedUser, value);
    }

    /// <summary>
    /// Asynchronously initializes the view model by fetching the authentication token, validating it and loading the authenticated user.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    private async Task InitializeAsync()
    {
        TokenDto? token = await _authenticationService.GetTokenAsync();
        if (token == null || !AuthenticationService.ValidateToken(token))
        {
            await _dialogService.DisplayUnauthorizedAlertAsync();
            await _navigationService.NavigateToLoginPageAsync();

            return;
        }

        AuthenticatedUser = new UserViewModel(token);
    }

    /// <summary>
    /// Asynchronously logs the user out and revoking their token.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [RelayCommand(AllowConcurrentExecutions = false)]
    public async Task LogoutAsync()
    {
        if (_authenticationService.RevokeToken())
        {
            await _navigationService.NavigateToLoginPageAsync();
        }
    }
}
