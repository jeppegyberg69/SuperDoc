using SuperDoc.Shared.ViewModels.Pages;

namespace SuperDoc.Application.Views;

public partial class ProfilePage : ContentPage
{
    public ProfilePage()
    {
        InitializeComponent();
    }

    public ProfilePageViewModel? ViewModel { get => BindingContext as ProfilePageViewModel; }

    protected override void OnNavigatedFrom(NavigatedFromEventArgs args)
    {
        base.OnNavigatedFrom(args);
    }
}