using System.Collections.ObjectModel;

using CommunityToolkit.Mvvm.Input;

using SuperDoc.Shared.Exceptions;
using SuperDoc.Shared.Models.Documents;
using SuperDoc.Shared.Models.Users;
using SuperDoc.Shared.Services;
using SuperDoc.Shared.Services.Contracts;
using SuperDoc.Shared.ViewModels.Wrappers;

namespace SuperDoc.Shared.ViewModels.Pages;

public partial class CaseDetailsPageViewModel(ICaseService caseService, IAuthenticationService authenticationService, INavigationService navigationService, IDialogService dialogService) : BaseViewModel
{
    private UserViewModel? _user;
    public UserViewModel? User
    {
        get => _user;
        private set => SetProperty(ref _user, value);
    }

    private CaseViewModel _case = null!;
    public CaseViewModel Case
    {
        get => _case;
        private set => SetProperty(ref _case, value);
    }

    private ObservableCollection<DocumentViewModel> _documents = new ObservableCollection<DocumentViewModel>();
    public ObservableCollection<DocumentViewModel> Documents
    {
        get => _documents;
        private set => SetProperty(ref _documents, value);
    }

    /// <summary>
    /// Asynchronously loads case details and associated documents.
    /// </summary>
    /// <param name="case">The case to load.</param>
    /// <param name="cancellationToken">An optinal cancellation token for the asynchronous operation.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [RelayCommand(AllowConcurrentExecutions = false)]
    private async Task LoadCaseAsync(CaseViewModel? @case, CancellationToken cancellationToken = default)
    {
        TokenDto? token = await authenticationService.GetTokenAsync();
        if (token == null || !AuthenticationService.ValidateToken(token))
        {
            await dialogService.DisplayUnauthorizedAlertAsync();
            await navigationService.NavigateToLoginPageAsync();

            return;
        }

        if (@case == null)
        {
            await dialogService.DisplayErrorAlertAsync("Det var desværre ikke muligt at indlæse sagen, prøv venligst igen senere.");
            await navigationService.GoBackAsync();

            return;
        }

        User = new UserViewModel(token);
        Case = @case;

        caseService.SetAuthorization(token.Token);
        await LoadDocumentsAsync(cancellationToken);
    }

    /// <summary>
    /// Asynchronously loads documents associated with the current case to which the authenticated user is assigned to.
    /// </summary>
    /// <param name="cancellationToken">An optinal cancellation token for the asynchronous operation.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [RelayCommand(AllowConcurrentExecutions = false)]
    private async Task LoadDocumentsAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            IEnumerable<DocumentDto> documents = await caseService.GetCaseDocumentsAsync(Case.CaseId, cancellationToken);
            Documents = new ObservableCollection<DocumentViewModel>(documents.Select(document => new DocumentViewModel(document)));
        }
        catch (HttpServiceException exception)
        {
            await dialogService.DisplayErrorAlertAsync(exception.Message);
        }
    }

    /// <summary>
    /// Asynchronously displays detailed information about the current case in an dialog.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [RelayCommand(AllowConcurrentExecutions = false)]
    private async Task ShowCaseInformationAsync()
    {
        await dialogService.DisplayAlertAsync("Sagsoplysninger", $"Sagsnummer: {Case?.CaseNumber}\n\nTitel: {Case?.Title}\n\nAnsvarlig sagsbehandler: {Case?.ResponsibleCaseManager?.FullName}\n\nSagsbehandlere: {string.Join(", ", Case?.CaseManagers?.Select(x => x.FullName) ?? Enumerable.Empty<string>())}\n\nBeskrivelse: {Case?.Description}", "OK");
    }

    /// <summary>
    /// Asynchronously opens the specified document, navigating to the document page.
    /// </summary>
    /// <param name="document">The document to be opened.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [RelayCommand(AllowConcurrentExecutions = false)]
    private async Task OpenDocumentAsync(DocumentViewModel document)
    {
        await navigationService.NavigateToPageAsync("Document", new Dictionary<string, object>() { { "Document", document } });
    }
}
