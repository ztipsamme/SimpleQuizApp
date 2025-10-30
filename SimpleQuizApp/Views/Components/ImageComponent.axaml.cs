using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media.Imaging;

namespace SimpleQuizApp.Views.Components;

public partial class ImageComponent : UserControl
{
    public static readonly StyledProperty<Bitmap?> SourceProperty =
        AvaloniaProperty.Register<ImageComponent, Bitmap?>(nameof(Source));

    public Bitmap? Source
    {
        get => GetValue(SourceProperty);
        set => SetValue(SourceProperty, value);
    }

    public static readonly StyledProperty<bool> HasImageProperty =
        AvaloniaProperty.Register<ImageComponent, bool>(nameof(HasImage));

    public bool HasImage
    {
        get => GetValue(HasImageProperty);
        set => SetValue(HasImageProperty, value);
    }

    public static readonly StyledProperty<string?> AspectRatioProperty =
        AvaloniaProperty.Register<ImageComponent, string?>(nameof
            (AspectRatio));

    public string? AspectRatio
    {
        get => GetValue(AspectRatioProperty);
        set => SetValue(AspectRatioProperty, value);
    }

    public static readonly StyledProperty<string?> RadiusProperty =
        AvaloniaProperty.Register<ImageComponent, string?>(nameof
            (Radius));

    public string? Radius
    {
        get => GetValue(RadiusProperty);
        set => SetValue(RadiusProperty, value);
    }

    public ImageComponent()
    {
        InitializeComponent();

        if (AspectRatio != null)
        {
            double[] numbers = AspectRatio
                .Split(',')
                .Select(x => double.Parse(x))
                .ToArray();

            double h = numbers[0];
            double w = numbers[1];

            Image.SizeChanged += (s, e) =>
            {
                double width = Image.Bounds.Width;
                double aspectRatio = h / w;
                Image.Height = width * aspectRatio;
            };
        }

        if (Radius != null)
        {
            var n = Radius
                .Split(',')
                .Select(x => double.Parse(x))
                .ToArray();

            if (n.Length == 1) Image.CornerRadius = new CornerRadius(n[0]);
            if (n.Length == 2) Image.CornerRadius = new CornerRadius(n[0], n[1]);
            if (n.Length == 4) Image.CornerRadius = new CornerRadius(n[0], n[1], n[2], n[3]);
        }
    }
}