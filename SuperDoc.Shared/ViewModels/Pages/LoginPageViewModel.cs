using System.ComponentModel.DataAnnotations;

using CommunityToolkit.Mvvm.Input;

using SuperDoc.Shared.Exceptions;
using SuperDoc.Shared.Models.Users;
using SuperDoc.Shared.Services;

namespace SuperDoc.Shared.ViewModels.Pages;

public partial class LoginPageViewModel(IAuthenticationService authenticationService, INavigationService navigationService) : BaseViewModel
{
    private string _emailAddress = string.Empty;
    [EmailAddress(ErrorMessage = "Indtast en gyldig email adresse")]
    public string EmailAddress
    {
        get => _emailAddress;
        set => SetProperty(ref _emailAddress, value, true);
    }

    private string _password = string.Empty;
    [MinLength(1, ErrorMessage = "Adgangskoden skal være på mellem 8 og 128 tegn")]
    public string Password
    {
        get => _password;
        set => SetProperty(ref _password, value, true);
    }

    private bool _isEmailAddressValid;
    public bool IsEmailAddressValid
    {
        get => _isEmailAddressValid;
        set => SetProperty(ref _isEmailAddressValid, value);
    }

    private bool _isPasswordValid;
    public bool IsPasswordValid
    {
        get => _isPasswordValid;
        set => SetProperty(ref _isPasswordValid, value);
    }

    private bool _isInvalidLogin;
    public bool IsInvalidLogin
    {
        get => _isInvalidLogin;
        private set => SetProperty(ref _isInvalidLogin, value);
    }

    [RelayCommand(AllowConcurrentExecutions = false)]
    private async Task ValidateAuthenticationTokenAsync()
    {
        bool isAuthenticated = await authenticationService.CheckAuthenticationAsync();
        if (isAuthenticated)
        {
            await navigationService.NavigateToMainPageAsync();
        }
    }

    [RelayCommand(AllowConcurrentExecutions = false)]
    private async Task LoginAsync(CancellationToken cancellationToken)
    {
        ValidateAllProperties();
        if (HasErrors)
        {
            return;
        }

        TokenDto? result = default;
        try
        {
            result = await authenticationService.LoginAsync(EmailAddress, Password, cancellationToken);
        }
        catch (HttpServiceException)
        {
            // Something went wrong with attempting to login, please try again.
        }

        if (result != null)
        {
            await navigationService.NavigateToMainPageAsync();
        }

        IsInvalidLogin = result == null;
    }
}
