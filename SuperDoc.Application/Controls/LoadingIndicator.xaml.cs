namespace SuperDoc.Application.Controls;

/// <summary>
/// An extended <see cref="ActivityIndicator"/> with text that is designed to be used as a loading indicator.
/// </summary>
public partial class LoadingIndicator : ContentView
{
    public LoadingIndicator()
    {
        InitializeComponent();
    }

    public string Text
    {
        get => (string)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    public static readonly BindableProperty TextProperty = BindableProperty.Create(nameof(Text), typeof(string), typeof(LoadingIndicator), string.Empty);
}