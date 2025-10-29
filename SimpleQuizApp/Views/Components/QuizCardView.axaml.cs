using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace SimpleQuizApp.Views.Components;

public partial class QuizCardView : UserControl
{
    public QuizCardView()
    {
        InitializeComponent();

        QuizCard.SizeChanged += (s, e) =>
        {
            double width = QuizCard.Bounds.Width;
            double aspectRatio = 3.0 / 4.0;
            QuizCard.Height = width * aspectRatio;
        };
    }
}