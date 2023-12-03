using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

using CommunityToolkit.Mvvm.Input;

namespace SuperDoc.Application.Controls;

/// <summary>
/// An <see cref="Entry"/> that is designed to work with data binding and supports the INotifyDataErrorInfo interface for handling and displaying validation errors.
/// </summary>
public partial class ValidationEntry : ContentView
{
    // Holds a reference to the current binding context that implements the INotifyDataErrorInfo interface.
    private INotifyDataErrorInfo? _currentBindingContext;

    public ValidationEntry()
    {
        InitializeComponent();

        // Set the initial value of _currentBindingContext to the BindingContext if it implements INotifyDataErrorInfo.
        _currentBindingContext = BindingContext as INotifyDataErrorInfo;

        // Subscribe to the BindingContextChanged event to handle changes in the binding context.
        BindingContextChanged += ValidationEntry_BindingContextChanged;
    }

    public string Header
    {
        get => (string)GetValue(HeaderProperty);
        set => SetValue(HeaderProperty, value);
    }

    public string Text
    {
        get => (string)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    public string Placeholder
    {
        get => (string)GetValue(PlaceholderProperty);
        set => SetValue(PlaceholderProperty, value);
    }

    public Keyboard Keyboard
    {
        get => (Keyboard)GetValue(KeyboardProperty);
        set => SetValue(KeyboardProperty, value);
    }

    public bool IsPassword
    {
        get => (bool)GetValue(IsPasswordProperty);
        set => SetValue(IsPasswordProperty, value);
    }

    public string ValidationPropertyName
    {
        get => (string)GetValue(ValidationPropertyNameProperty);
        set => SetValue(ValidationPropertyNameProperty, value);
    }

    public string? ErrorMessage
    {
        get => (string)GetValue(ErrorMessageProperty);
        private set => SetValue(ErrorMessageProperty, value);
    }

    public bool IsValid
    {
        get => (bool)GetValue(IsValidProperty);
        set => SetValue(IsValidProperty, value);
    }

    public static readonly BindableProperty HeaderProperty = BindableProperty.Create(nameof(Header), typeof(string), typeof(ValidationEntry), string.Empty);

    public static readonly BindableProperty TextProperty = BindableProperty.Create(nameof(Text), typeof(string), typeof(ValidationEntry), string.Empty);

    public static readonly BindableProperty PlaceholderProperty = BindableProperty.Create(nameof(Placeholder), typeof(string), typeof(ValidationEntry), string.Empty);

    public static readonly BindableProperty KeyboardProperty = BindableProperty.Create(nameof(Keyboard), typeof(Keyboard), typeof(ValidationEntry), Keyboard.Default);

    public static readonly BindableProperty IsPasswordProperty = BindableProperty.Create(nameof(IsPassword), typeof(bool), typeof(ValidationEntry), default(bool));

    public static readonly BindableProperty ValidationPropertyNameProperty = BindableProperty.Create(nameof(ValidationPropertyName), typeof(string), typeof(ValidationEntry), string.Empty, propertyChanged: ValidationPropertyName_PropertyChanged);

    public static readonly BindableProperty ErrorMessageProperty = BindableProperty.Create(nameof(ErrorMessage), typeof(string), typeof(ValidationEntry), null);

    public static readonly BindableProperty IsValidProperty = BindableProperty.Create(nameof(IsValid), typeof(bool), typeof(ValidationEntry), false);

    private void ValidationEntry_BindingContextChanged(object? sender, EventArgs e)
    {
        // Unsubscribe from the ErrorsChanged event of the previous binding context, if any.
        if (_currentBindingContext != null)
        {
            _currentBindingContext.ErrorsChanged -= BindingContext_ErrorsChanged;
        }

        // Check if the new binding context implements INotifyDataErrorInfo.
        if (BindingContext is INotifyDataErrorInfo newBindingContext)
        {
            // Subscribe to the ErrorsChanged event of the new binding context, so that we can be notified of any errors.
            newBindingContext.ErrorsChanged += BindingContext_ErrorsChanged;
            _currentBindingContext = newBindingContext;
        }

        // Trigger property validation to show any existing errors in the new binding context.
        ValidateProperty();
    }

    private static void ValidationPropertyName_PropertyChanged(BindableObject obj, object oldValue, object newValue)
    {
        if (obj is ValidationEntry validationEntry)
        {
            validationEntry.ValidateProperty();
        }
    }

    private void BindingContext_ErrorsChanged(object? sender, DataErrorsChangedEventArgs e)
    {
        ValidateProperty();
    }

    [RelayCommand] // Automatically generate an ICommand property from the declared method.
    private void ValidateProperty()
    {
        // Get validation errors for the specified property by the ValidationPropertyName.
        ValidationResult? result = _currentBindingContext?.GetErrors(ValidationPropertyName).OfType<ValidationResult>().FirstOrDefault();

        // Set the ErrorMessage property based on the validation result.
        ErrorMessage = result?.ErrorMessage;
    }
}