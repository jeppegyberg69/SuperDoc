using System.Collections.ObjectModel;

using CommunityToolkit.Mvvm.Input;

using SuperDoc.Shared.Exceptions;
using SuperDoc.Shared.Models.Cases;
using SuperDoc.Shared.Models.Users;
using SuperDoc.Shared.Services;
using SuperDoc.Shared.Services.Contracts;
using SuperDoc.Shared.ViewModels.Wrappers;

namespace SuperDoc.Shared.ViewModels.Pages;

public partial class CasesPageViewModel : BaseViewModel
{
    private readonly IAuthenticationService _authenticationService;
    private readonly INavigationService _navigationService;
    private readonly IDialogService _dialogService;

    private readonly ICaseService _caseService;

    public CasesPageViewModel(ICaseService caseService, IAuthenticationService authenticationService, INavigationService navigationService, IDialogService dialogService)
    {
        _authenticationService = authenticationService;
        _navigationService = navigationService;
        _dialogService = dialogService;

        _caseService = caseService;

        Initialization = InitializeAsync();
    }

    private ObservableCollection<CaseViewModel> _cases = new ObservableCollection<CaseViewModel>();
    public ObservableCollection<CaseViewModel> Cases
    {
        get => _cases;
        private set => SetProperty(ref _cases, value);
    }

    /// <summary>
    /// Asynchronously initializes the view model by fetching the authentication token, validating it, and loading cases if successful.
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

        _caseService.SetAuthorization(token.Token);
        await LoadCasesAsync();
    }

    /// <summary>
    /// Asynchronously loads cases to which the authenticated user is assigned to.
    /// </summary>
    /// <param name="cancellationToken">An optinal cancellation token for the asynchronous operation.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [RelayCommand(AllowConcurrentExecutions = false)]
    private async Task LoadCasesAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            IEnumerable<CaseDto> cases = await _caseService.GetCasesAsync(cancellationToken);
            Cases = new ObservableCollection<CaseViewModel>(cases.Select(@case => new CaseViewModel(@case)));
        }
        catch (HttpServiceException exception)
        {
            await _dialogService.DisplayErrorAlertAsync(exception.Message);
        }
    }

    /// <summary>
    /// Asynchronously opens the specified case, navigating to the case details page.
    /// </summary>
    /// <param name="case">The case to be opened.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [RelayCommand(AllowConcurrentExecutions = false)]
    private async Task OpenCaseAsync(CaseViewModel @case)
    {
        await _navigationService.NavigateToPageAsync("Details", new Dictionary<string, object>() { { "Case", @case } });
    }
}
