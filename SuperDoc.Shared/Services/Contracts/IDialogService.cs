namespace SuperDoc.Shared.Services.Contracts;

/// <summary>
/// Service responsible for displaying various types of alert dialogs within the application.
/// </summary>
public interface IDialogService
{
    /// <summary>
    /// Displays a simple alert dialog with a title, message, and a single button.
    /// </summary>
    /// <param name="title">The title of the alert.</param>
    /// <param name="message">The message to be displayed in the alert.</param>
    /// <param name="cancel">The text for the cancel button.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public Task DisplayAlertAsync(string title, string message, string cancel);

    /// <summary>
    /// Displays a confirmation alert dialog with a title, message, and two buttons.
    /// </summary>
    /// <param name="title">The title of the alert.</param>
    /// <param name="message">The message to be displayed in the alert.</param>
    /// <param name="accept">The text for the accept button.</param>
    /// <param name="cancel">The text for the cancel button.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation, containing a boolean result indicating the user's choice.</returns>
    public Task<bool> DisplayAlertAsync(string title, string message, string accept, string cancel);

    /// <summary>
    /// Displays an error alert dialog with a predefined title, an error message, and an "OK" button.
    /// </summary>
    /// <param name="message">The error message to be displayed in the alert.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public Task DisplayErrorAlertAsync(string message);

    /// <summary>
    /// Displays an unauthorized access alert dialog with a predefined title, message, and an "OK" button.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public Task DisplayUnauthorizedAlertAsync();
}
