using CommunityToolkit.Mvvm.ComponentModel;

namespace SuperDoc.Shared.ViewModels;

public abstract class BaseViewModel : ObservableValidator
{
    public BaseViewModel()
    {
        Initialization = Task.CompletedTask;
    }

    private TaskNotifier _initialization = null!;
    public Task Initialization
    {
        get => _initialization!;
        set => SetPropertyAndNotifyOnCompletion(ref _initialization, value);
    }
}
