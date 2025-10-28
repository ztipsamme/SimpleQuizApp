using Avalonia;
using Avalonia.Controls;

namespace SimpleQuizApp.Views.Components;

public enum Color
{
    Primary,
    Default,
    Ghost,
    Danger
}

public enum Size
{
    Small,
    Medium,
    Large,
}

public class VariantButton : Button
{
    public static readonly StyledProperty<Color> ColorProperty =
        AvaloniaProperty.Register<VariantButton, Color>(nameof(Color), Color.Default);

    public static readonly StyledProperty<Size> SizeProperty =
        AvaloniaProperty.Register<VariantButton, Size>(nameof(Size),
            Size.Medium);

    public Color Color
    {
        get => GetValue(ColorProperty);
        set => SetValue(ColorProperty, value);
    }
    
    public Size Size
    {
        get => GetValue(SizeProperty);
        set => SetValue(SizeProperty, value);
    }
}