using SuperDoc.Shared.Services.Contracts;

namespace SuperDoc.Application.Services;

/// <inheritdoc cref="IDialogService"/>
public class DialogService : IDialogService
{
    /// <inheritdoc cref="IDialogService.DisplayAlertAsync(string, string, string)"/>
    public async Task DisplayAlertAsync(string title, string message, string cancel)
    {
        await Shell.Current.CurrentPage.DisplayAlert(title, message, cancel);
    }

    /// <inheritdoc cref="IDialogService.DisplayAlertAsync(string, string, string, string)"/>
    public async Task<bool> DisplayAlertAsync(string title, string message, string accept, string cancel)
    {
        return await Shell.Current.CurrentPage.DisplayAlert(title, message, accept, cancel);
    }

    /// <inheritdoc cref="IDialogService.DisplayErrorAlertAsync(string)"/>
    public async Task DisplayErrorAlertAsync(string message)
    {
        await DisplayAlertAsync("Hovsa, der opstod en fejl...", message, "OK");
    }

    /// <inheritdoc cref="IDialogService.DisplayUnauthorizedAlertAsync"/>
    public async Task DisplayUnauthorizedAlertAsync()
    {
        await DisplayAlertAsync("Hovsa, adgang nægtet...", "Din session er udløbet, og du kan derfor ikke tilgå denne side. Prøv venligst igen efter at du har logget ind.", "OK");
    }
}
