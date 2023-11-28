using System.Diagnostics;
using System.Reflection;
using System.Xml.Linq;

using SuperDoc.Shared.Helpers;

namespace SuperDoc.Application.Helpers;

public static class ViewModelLocator
{
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

    public static TViewModel? FindViewModel<TViewModel>(Type viewType)
    {
        return (TViewModel?)FindViewModel(viewType);
    }

    public static object? FindViewModel(Type viewType)
    {
        string viewModelName = $"{viewType.Name}ViewModel";

        Type? viewModelType = AssemblyHelper.GetType(viewModelName);
        if (viewModelType == null)
        {
            Debug.WriteLine($"{viewModelName} was not found.");
            return default;
        }

        ServiceHelper.EnsureServicesInitialized();
        return ActivatorUtilities.GetServiceOrCreateInstance(ServiceHelper.Services, viewModelType);
    }
}
