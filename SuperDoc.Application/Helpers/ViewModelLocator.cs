using System.Diagnostics;

using SuperDoc.Shared.Helpers;

namespace SuperDoc.Application.Helpers;

/// <summary>
/// Helper class for associating view models with views within the application.
/// </summary>
public static class ViewModelLocator
{
    // BindableProperty to handle attaching and detaching view models.
    public static readonly BindableProperty InitializeViewModelProperty =
        BindableProperty.CreateAttached("InitializeViewModel", typeof(bool), typeof(ViewModelLocator), default(bool), propertyChanged: OnInitializeViewModelChanged);

    public static bool GetInitializeViewModel(BindableObject obj)
    {
        return (bool)obj.GetValue(InitializeViewModelProperty);
    }

    public static void SetInitializeViewModel(BindableObject obj, bool value)
    {
        obj.SetValue(InitializeViewModelProperty, value);
    }

    private static void OnInitializeViewModelChanged(BindableObject obj, object oldValue, object newValue)
    {
        if (obj is Element view)
        {
            InitializeViewModel(view);
        }
    }

    private static void InitializeViewModel(Element view)
    {
        view.BindingContext = FindViewModel(view.GetType());
    }

    /// <summary>
    /// Finds and returns a strongly typed view model for a given view type.
    /// </summary>
    /// <typeparam name="TViewModel">The type of the view model to retrieve.</typeparam>
    /// <param name="viewType">The type of the view associated with the view model.</param>
    /// <returns>The retrieved view model or <see langword="null"/> if the view model is not found.</returns>
    public static TViewModel? FindViewModel<TViewModel>(Type viewType)
    {
        return (TViewModel?)FindViewModel(viewType);
    }

    /// <summary>
    /// Finds and returns a view model for a given view type.
    /// </summary>
    /// <param name="viewType">The type of the view associated with the view model.</param>
    /// <returns>The retrieved view model or <see langword="null"/> if the view model is not found.</returns>
    public static object? FindViewModel(Type viewType)
    {
        // The expected view model name based on the view type.
        string viewModelName = $"{viewType.Name}ViewModel";

        // Try to get the Type of the view model based on the constructed name.
        Type? viewModelType = AssemblyHelper.GetType(viewModelName);
        if (viewModelType == null)
        {
            Debug.WriteLine($"{viewModelName} was not found.");
            return default;
        }

        // Ensure that ServiceHelper is initialized before creating an instance of the view model.
        ServiceHelper.EnsureServicesInitialized();

        // Create an instance of the view model using the ServiceHelper to resolve dependencies.
        return ActivatorUtilities.GetServiceOrCreateInstance(ServiceHelper.Services, viewModelType);
    }
}
