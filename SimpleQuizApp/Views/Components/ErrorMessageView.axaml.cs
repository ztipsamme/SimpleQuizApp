using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace SimpleQuizApp.Views.Components;

public partial class ErrorMessageView : UserControl
{
    public static readonly StyledProperty<bool> IsVisibleProperty =
        AvaloniaProperty.Register<ErrorMessageView, bool>(nameof(IsVisible));

    public static readonly StyledProperty<string> MessageProperty =
        AvaloniaProperty.Register<ErrorMessageView, string>(nameof(Message));

    public bool IsVisible
    {
        get => GetValue(IsVisibleProperty);
        set => SetValue(IsVisibleProperty, value);
    }

    public string Message
    {
        get => GetValue(MessageProperty);
        set => SetValue(MessageProperty, value);
    }

    public ErrorMessageView()
    {
        InitializeComponent();
    }
}