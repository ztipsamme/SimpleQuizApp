using Avalonia;
using Avalonia.Controls;

namespace SimpleQuizApp.Views.Components;

public partial class InputFieldView : UserControl
{
    public static readonly StyledProperty<string> LabelProperty =
        AvaloniaProperty.Register<InputFieldView, string>(nameof(Label));

    public static readonly StyledProperty<string> DescriptionProperty =
        AvaloniaProperty.Register<InputFieldView, string>(nameof(Description));

    public static readonly StyledProperty<string> TextProperty =
        AvaloniaProperty.Register<InputFieldView, string>(nameof(Text));

    public string Label
    {
        get => GetValue(LabelProperty);
        set => SetValue(LabelProperty, value);
    }

    public string Description
    {
        get => GetValue(DescriptionProperty);
        set => SetValue(DescriptionProperty, value);
    }

    public string Text
    {
        get => GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    public InputFieldView()
    {
        InitializeComponent();
    }
}