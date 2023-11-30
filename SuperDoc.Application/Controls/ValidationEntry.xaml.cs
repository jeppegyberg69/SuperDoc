using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

using CommunityToolkit.Mvvm.Input;

namespace SuperDoc.Application.Controls;

public partial class ValidationEntry : ContentView
{
    private INotifyDataErrorInfo? _currentBindingContext;

    public ValidationEntry()
    {
        InitializeComponent();

        _currentBindingContext = BindingContext as INotifyDataErrorInfo;
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

    private static void ValidationPropertyName_PropertyChanged(BindableObject obj, object oldValue, object newValue)
    {
        if (obj is ValidationEntry validationEntry)
        {
            validationEntry.ValidateProperty();
        }
    }

    private void ValidationEntry_BindingContextChanged(object? sender, EventArgs e)
    {
        if (_currentBindingContext != null)
        {
            _currentBindingContext.ErrorsChanged -= BindingContext_ErrorsChanged;
        }

        if (BindingContext is INotifyDataErrorInfo newBindingContext)
        {
            newBindingContext.ErrorsChanged += BindingContext_ErrorsChanged;
            _currentBindingContext = newBindingContext;
        }

        ValidateProperty();
    }

    private void BindingContext_ErrorsChanged(object? sender, DataErrorsChangedEventArgs e)
    {
        ValidateProperty();
    }

    [RelayCommand]
    private void SetTextProperty(string text)
    {
        // UpdateSourceTrigger doesn't exist in MAUI, so we'll have to recreate it with UserStoppedTypingBehavior.
        Text = text;
    }

    [RelayCommand]
    private void ValidateProperty()
    {
        ValidationResult? result = _currentBindingContext?.GetErrors(ValidationPropertyName).OfType<ValidationResult>().FirstOrDefault();
        ErrorMessage = result?.ErrorMessage;
    }
}