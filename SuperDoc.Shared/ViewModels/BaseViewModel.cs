using System.ComponentModel;

using CommunityToolkit.Mvvm.ComponentModel;

namespace SuperDoc.Shared.ViewModels;

/// <summary>
/// Provides a base abstract class for view models, encapsulating common functionality and structure.
/// </summary>
public abstract class BaseViewModel : ObservableValidator
{
    /// <summary>
    /// Initializes a new instance of the <see cref="BaseViewModel"/> class.
    /// </summary>
    /// <remarks>
    /// This constructor sets the <see cref="Initialization"/> property to a completed task by default.
    /// </remarks>
    public BaseViewModel()
    {
        Initialization = Task.CompletedTask;
    }

    private TaskNotifier _initialization = null!;
    /// <summary>
    /// Gets or sets the asynchronous task representing the initialization process.
    /// </summary>
    /// <remarks>
    /// The initialization task is used to perform asynchronous initialization operations from the constructor.
    /// </remarks>
    public Task Initialization
    {
        get => _initialization!;
        protected set => SetPropertyAndNotifyOnCompletion(ref _initialization, value);
    }

    /// <summary>
    /// Gets whether the view model has initialized.
    /// </summary>
    public bool IsInitialized
    {
        get => Initialization.IsCompleted;
    }

    /// <summary>
    /// Gets whether the view model has initialized sucessfully.
    /// </summary>
    public bool IsInitializedSucessfully
    {
        get => Initialization.IsCompletedSuccessfully;
    }

    protected override void OnPropertyChanged(PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(Initialization))
        {
            // Notify changes in dependent properties related to initialization.
            OnPropertyChanged(nameof(IsInitialized));
            OnPropertyChanged(nameof(IsInitializedSucessfully));
        }

        base.OnPropertyChanged(e);
    }
}
