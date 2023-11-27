namespace SuperDoc.Application.Views;

public partial class CaseDetailsPage : ContentPage
{
    public CaseDetailsPage()
    {
        InitializeComponent();
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