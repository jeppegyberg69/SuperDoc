using CommunityToolkit.Mvvm.Input;

using SuperDoc.Shared.Models.Users;
using SuperDoc.Shared.Services;
using SuperDoc.Shared.ViewModels.Wrappers;

namespace SuperDoc.Shared.ViewModels.Pages;

public partial class ProfilePageViewModel : BaseViewModel
{
    private readonly IAuthenticationService _authenticationService;

    private readonly INavigationService _navigationService;

    public ProfilePageViewModel(IAuthenticationService authenticationService, INavigationService navigationService)
    {
        _authenticationService = authenticationService;
        _navigationService = navigationService;

        Initialization = InitializeAsync();
    }

    private async Task InitializeAsync()
    {
        TokenDto? token = await _authenticationService.GetTokenAsync();
        if (token == null)
        {
            // Show error message
            return;
        }

        User = new UserViewModel(token);
    }

    private UserViewModel? _user;
    public UserViewModel? User
    {
        get => _user;
        private set => SetProperty(ref _user, value);
    }

    [RelayCommand(AllowConcurrentExecutions = false)]
    public async Task LogoutAsync()
    {
        if (_authenticationService.RevokeToken())
        {
            await _navigationService.NavigateToLoginPageAsync();
        }
    }
}
