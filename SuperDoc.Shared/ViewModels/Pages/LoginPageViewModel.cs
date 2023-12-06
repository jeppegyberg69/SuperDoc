using System.ComponentModel.DataAnnotations;

using CommunityToolkit.Mvvm.Input;

using SuperDoc.Shared.Exceptions;
using SuperDoc.Shared.Models.Users;
using SuperDoc.Shared.Services.Contracts;

namespace SuperDoc.Shared.ViewModels.Pages;

public partial class LoginPageViewModel(IAuthenticationService authenticationService, INavigationService navigationService, IDialogService dialogService) : BaseViewModel
{
    private string _emailAddress = string.Empty;
    [EmailAddress(ErrorMessage = "Indtast en gyldig e-mailadresse")]
    public string EmailAddress
    {
        get => _emailAddress;
        set => SetProperty(ref _emailAddress, value, false);
    }

    private string _password = string.Empty;
    [StringLength(128, MinimumLength = 8, ErrorMessage = "Adgangskoden skal være på mellem 8 og 128 tegn")]
    public string Password
    {
        get => _password;
        set => SetProperty(ref _password, value, false);
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
        private set
        {
            if (SetProperty(ref _isInvalidLogin, value) && IsInvalidLogin)
            {
                Password = string.Empty;
                ClearErrors(nameof(Password));
            }
        }
    }

    /// <summary>
    /// Asynchronously validates the authentication token.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [RelayCommand(AllowConcurrentExecutions = false)]
    private async Task ValidateAuthenticationTokenAsync()
    {
        bool isAuthenticated = await authenticationService.CheckAuthenticationAsync();
        if (isAuthenticated)
        {
            await navigationService.NavigateToMainPageAsync();
        }
    }

    /// <summary>
    /// Asynchronously initiates the login process.
    /// </summary>
    /// <param name="cancellationToken">An optinal cancellation token for the asynchronous operation.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [RelayCommand(AllowConcurrentExecutions = false)]
    private async Task LoginAsync(CancellationToken cancellationToken = default)
    {
        IsInvalidLogin = false;

        // Validate properties and return if there are errors, so that we don't attempt to call the API with known invalid data.
        ValidateAllProperties();
        if (HasErrors)
        {
            return;
        }

        TokenDto? token;
        try
        {
            token = await authenticationService.LoginAsync(EmailAddress, Password, cancellationToken);
        }
        catch (HttpServiceException)
        {
            await dialogService.DisplayErrorAlertAsync("Der opstod tekniske problemer under forsøget på at logge ind. Prøver venligst igen om et øjeblik.");

            return;
        }

        if (token != null)
        {
            await navigationService.NavigateToMainPageAsync();
        }

        IsInvalidLogin = token == null;
    }
}
