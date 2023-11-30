using SuperDoc.Shared.ViewModels.Pages;

namespace SuperDoc.Application.Views;

public partial class LoginPage : ContentPage
{
    public LoginPage()
    {
        InitializeComponent();
    }

    public LoginPageViewModel? ViewModel { get => BindingContext as LoginPageViewModel; }

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        ViewModel?.ValidateAuthenticationTokenCommand.ExecuteAsync(null);

        base.OnNavigatedTo(args);
    }
}