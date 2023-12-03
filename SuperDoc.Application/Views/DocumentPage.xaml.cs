using SuperDoc.Shared.ViewModels.Pages;
using SuperDoc.Shared.ViewModels.Wrappers;

namespace SuperDoc.Application.Views;

[QueryProperty(nameof(Document), nameof(Document))]
public partial class DocumentPage : ContentPage
{
    public DocumentPage()
    {
        InitializeComponent();
    }

    public DocumentPageViewModel? ViewModel { get => BindingContext as DocumentPageViewModel; }

    public DocumentViewModel? Document
    {
        get => ViewModel?.Document;
        set
        {
            // Check if the ViewModel is not null and if the LoadDocumentCommand can be executed with the provided document.
            if (ViewModel != null && ViewModel.LoadDocumentCommand.CanExecute(value))
            {
                // Execute the LoadCaseCommand with the provided document.
                ViewModel.LoadDocumentCommand.Execute(value);
            }
        }
    }

    protected override void OnDisappearing()
    {
        CloseMoreButtonFlyoutMenu();
        base.OnDisappearing();
    }

    private void Page_Tapped(object sender, TappedEventArgs e)
    {
        CloseMoreButtonFlyoutMenu();
    }

    private void MoreButton_Clicked(object sender, EventArgs e)
    {
        OpenMoreButtonFlyoutMenu();
    }

    private void FlyoutButton_Clicked(object sender, EventArgs e)
    {
        CloseMoreButtonFlyoutMenu();
    }

    private void EraseSignatureButton_Clicked(object sender, EventArgs e)
    {
        ClearSignaturePad();
    }

    private void SignDocumentButton_Clicked(object sender, EventArgs e)
    {
        ClearSignaturePad();
    }

    private void OpenMoreButtonFlyoutMenu()
    {
        MoreButtonFlyoutMenu.IsVisible = !MoreButtonFlyoutMenu.IsVisible;

        // Changing the visibility for the first time doesn't seem to cause MAUI to redraw the element, so we'll just force it every time to be sure.
        InvalidateMeasure();
    }

    private void CloseMoreButtonFlyoutMenu()
    {
        MoreButtonFlyoutMenu.IsVisible = false;
    }

    private void ClearSignaturePad()
    {
        DocumentSignaturePad.Clear();
    }
}