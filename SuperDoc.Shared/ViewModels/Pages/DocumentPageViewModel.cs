using System.Collections.ObjectModel;

using CommunityToolkit.Mvvm.Input;

using SuperDoc.Shared.Exceptions;
using SuperDoc.Shared.Helpers;
using SuperDoc.Shared.Models.Documents;
using SuperDoc.Shared.Models.Revisions;
using SuperDoc.Shared.Models.Users;
using SuperDoc.Shared.Services;
using SuperDoc.Shared.Services.Contracts;
using SuperDoc.Shared.ViewModels.Wrappers;

namespace SuperDoc.Shared.ViewModels.Pages;

public partial class DocumentPageViewModel(IDocumentService documentService, ISecureStorageService secureStorageService, IAuthenticationService authenticationService, INavigationService navigationService, IDialogService dialogService) : BaseViewModel
{
    private UserViewModel _authenticatedUser = null!;
    public UserViewModel AuthenticatedUser
    {
        get => _authenticatedUser;
        private set => SetProperty(ref _authenticatedUser, value);
    }

    private DocumentViewModel _document = null!;
    public DocumentViewModel Document
    {
        get => _document;
        private set => SetProperty(ref _document, value);
    }

    private ObservableCollection<RevisionViewModel> _revisions = new ObservableCollection<RevisionViewModel>();
    public ObservableCollection<RevisionViewModel> Revisions
    {
        get => _revisions;
        private set => SetProperty(ref _revisions, value);
    }

    private RevisionViewModel? _selectedRevision;
    public RevisionViewModel? SelectedRevision
    {
        get => _selectedRevision;
        set
        {
            if (SetProperty(ref _selectedRevision, value) && LoadRevisionCommand.CanExecute(value))
            {
                LoadRevisionCommand.Execute(value);
            }
        }
    }

    private Stream? _documentStream;
    public Stream? DocumentStream
    {
        get => _documentStream;
        private set => SetProperty(ref _documentStream, value);
    }

    private DocumentSignatureViewModel? _documentSignature;
    public DocumentSignatureViewModel? DocumentSignature
    {
        get => _documentSignature;
        private set => SetProperty(ref _documentSignature, value);
    }

    private bool _revisionHasSignatures;
    public bool RevisionHasSignatures
    {
        get => _revisionHasSignatures;
        private set => SetProperty(ref _revisionHasSignatures, value);
    }

    private bool _isSignatureRequired;
    public bool IsSignatureRequired
    {
        get => _isSignatureRequired;
        private set => SetProperty(ref _isSignatureRequired, value);
    }

    /// <summary>
    /// Asynchronously loads document and associated revisions.
    /// </summary>
    /// <param name="document">The document to load.</param>
    /// <param name="cancellationToken">An optinal cancellation token for the asynchronous operation.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [RelayCommand(AllowConcurrentExecutions = false)]
    private async Task LoadDocumentAsync(DocumentViewModel? document, CancellationToken cancellationToken = default)
    {
        TokenDto? token = await authenticationService.GetTokenAsync();
        if (token == null || !AuthenticationService.ValidateToken(token))
        {
            await dialogService.DisplayUnauthorizedAlertAsync();
            await navigationService.NavigateToLoginPageAsync();

            return;
        }

        if (document == null)
        {
            await dialogService.DisplayErrorAlertAsync("Det var desværre ikke muligt at indlæse dokumentet, prøv venligst igen senere.");
            await navigationService.GoBackAsync();

            return;
        }

        AuthenticatedUser = new UserViewModel(token);
        Document = document;

        documentService.SetAuthorization(AuthenticatedUser.Token);
        await LoadRevisionsAsync(cancellationToken);
    }

    /// <summary>
    /// Asynchronously loads the revisions associated with the current document.
    /// </summary>
    /// <param name="cancellationToken">An optinal cancellation token for the asynchronous operation.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    private async Task LoadRevisionsAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            RevisionDto[] revisions = (await documentService.GetDocumentRevisionsAsync(Document.DocumentId, cancellationToken)).ToArray();
            if (revisions.Length == 0)
            {
                await dialogService.DisplayErrorAlertAsync("Det var desværre ikke muligt at indlæse dokumentet, prøv venligst igen senere.");
                await navigationService.GoBackAsync();

                return;
            }

            // Iterate through the revisions and set the revision number then add them to the collection.
            for (int i = 0; i < revisions.Length; i++)
            {
                Revisions.Add(new RevisionViewModel(revisions[i])
                {
                    RevisionNumber = revisions.Length - i
                });
            }
        }
        catch (HttpServiceException exception)
        {
            await dialogService.DisplayErrorAlertAsync(exception.Message);
        }

        // Load the latest revision from the revision collection.
        SelectedRevision = Revisions.FirstOrDefault();
    }

    /// <summary>
    /// Asynchronously loads the details of a specific document revision.
    /// </summary>
    /// <param name="revision">The revision to load.</param>
    /// <param name="cancellationToken">An optinal cancellation token for the asynchronous operation.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [RelayCommand(AllowConcurrentExecutions = false)]
    private async Task LoadRevisionAsync(RevisionViewModel revision, CancellationToken cancellationToken = default)
    {
        try
        {
            Stream? documentStream = await documentService.GetDocumentStreamAsync(revision.RevisionId, cancellationToken);
            if (documentStream == null)
            {
                await dialogService.DisplayErrorAlertAsync("Det var desværre ikke muligt at indlæse dokumentet. Prøv venligst igen senere.");
            }

            DocumentStream = documentStream;
        }
        catch (HttpServiceException exception)
        {
            await dialogService.DisplayErrorAlertAsync(exception.Message);
        }

        UpdateDocumentSignatures();
    }

    /// <summary>
    /// Asynchronously signs the current document revision.
    /// </summary>
    /// <param name="cancellationToken">An optinal cancellation token for the asynchronous operation.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [RelayCommand(AllowConcurrentExecutions = false)]
    private async Task SignDocumentAsync(CancellationToken cancellationToken = default)
    {
        if (SelectedRevision == null || DocumentStream == null)
        {
            await dialogService.DisplayErrorAlertAsync("Det var desværre ikke muligt at underskrive dokumentet. Prøv venligst igen senere.");

            return;
        }

        // Try to retrieve the authenticated user's public and private keys from secure storage.
        string? publicKey = await secureStorageService.GetAsync(AuthenticatedUser.EmailAddress + SecureStorageHelper.PublicKeySufffix);
        string? privateKey = await secureStorageService.GetAsync(AuthenticatedUser.EmailAddress + SecureStorageHelper.PrivateKeySuffix);

        // If keys are not found, generate new ones and store them in secure storage.
        if (string.IsNullOrWhiteSpace(publicKey) || string.IsNullOrWhiteSpace(privateKey))
        {
            RSAKeys keys = CryptographicHelper.GenerateKeys();

            publicKey = keys.PublicKey;
            await secureStorageService.SetAsync(AuthenticatedUser.EmailAddress + SecureStorageHelper.PublicKeySufffix, publicKey);

            privateKey = keys.PrivateKey;
            await secureStorageService.SetAsync(AuthenticatedUser.EmailAddress + SecureStorageHelper.PrivateKeySuffix, privateKey);
        }

        // Hash and sign the document revision.
        string hashedRevision = await CryptographicHelper.HashStreamAsync(DocumentStream);
        string signedRevision = CryptographicHelper.SignData(Convert.FromHexString(hashedRevision), privateKey);

        try
        {
            DocumentSignatureDto? documentSignature = await documentService.SignDocumentRevisionAsync(SelectedRevision.RevisionId, signedRevision, publicKey, cancellationToken);
            if (documentSignature == null)
            {
                return;
            }

            // Find and update the corresponding signature in the SelectedRevision's Signatures collection.
            DocumentSignatureViewModel? signature = SelectedRevision.Signatures.FirstOrDefault(signature => signature.EmailAddress.Equals(documentSignature.EmailAddress));
            if (signature == null)
            {
                return;
            }

            SelectedRevision.Signatures.Remove(signature);
            SelectedRevision.Signatures.Add(new DocumentSignatureViewModel(documentSignature));

            // Update the document signatures to reflect the newly signed revision.
            UpdateDocumentSignatures();
        }
        catch (HttpServiceException exception)
        {
            await dialogService.DisplayErrorAlertAsync(exception.Message);
        }
    }

    /// <summary>
    /// Updates properties related to document signatures.
    /// </summary>
    private void UpdateDocumentSignatures()
    {
        if (SelectedRevision != null)
        {
            // Check if the selected revision has any signatures.
            RevisionHasSignatures = SelectedRevision.Signatures.Any();

            // Find the document signature for the authenticated user.
            DocumentSignature = SelectedRevision.Signatures.FirstOrDefault(signature => signature.EmailAddress.Equals(AuthenticatedUser.EmailAddress) && !string.IsNullOrWhiteSpace(signature.Signature));

            // Check if a signature is required by the authenticated user.
            IsSignatureRequired = SelectedRevision.Signatures.Any(signature => signature.EmailAddress.Equals(AuthenticatedUser.EmailAddress) && string.IsNullOrWhiteSpace(signature.Signature));
        }
    }

    /// <summary>
    /// Asynchronously displays information about the current document.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [RelayCommand(AllowConcurrentExecutions = false)]
    private async Task ShowDocumentInformationAsync()
    {
        await dialogService.DisplayAlertAsync("Dokumentoplysninger", $"Titel: {Document?.Title}\n\nEksterne brugere: {string.Join(", ", Document?.ExternalUsers?.Select(x => x.FullName) ?? Enumerable.Empty<string>())}\n\nBeskrivelse: {Document?.Description}", "OK");
    }

    /// <summary>
    /// Asynchronously displays information about signatories for the selected document revision.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [RelayCommand(AllowConcurrentExecutions = false)]
    private async Task ShowSignatoriesAsync()
    {
        if (SelectedRevision == null)
        {
            return;
        }

        // Create a message string to display information about each signatory.
        string message = string.Empty;

        foreach (DocumentSignatureViewModel signature in SelectedRevision.Signatures)
        {
            // Append information about each signatory to the message string.
            message += $"{signature.EmailAddress}: {(string.IsNullOrWhiteSpace(signature.Signature) ? "Mangler underskrift" : "Underskrevet")}\n\n";
        }

        await dialogService.DisplayAlertAsync("Underskrivere", message, "OK");
    }
}
