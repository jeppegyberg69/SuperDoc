using SuperDoc.Shared.ViewModels.Pages;
using SuperDoc.Shared.ViewModels.Wrappers;

namespace SuperDoc.Application.Views;

[QueryProperty(nameof(Case), nameof(Case))]
public partial class CaseDetailsPage : ContentPage
{
    public CaseDetailsPage()
    {
        InitializeComponent();
    }

    public CaseDetailsPageViewModel? ViewModel { get => BindingContext as CaseDetailsPageViewModel; }

    public CaseViewModel? Case
    {
        get => ViewModel?.Case;
        set
        {
            // Check if the ViewModel is not null and if the LoadCaseCommand can be executed with the provided case.
            if (ViewModel != null && ViewModel.LoadCaseCommand.CanExecute(value))
            {
                // Execute the LoadCaseCommand with the provided case.
                ViewModel.LoadCaseCommand.Execute(value);
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
}