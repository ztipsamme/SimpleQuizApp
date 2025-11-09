using System;
using System.Threading.Tasks;
using Avalonia.Media.Imaging;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SimpleQuizApp.Models;
using SimpleQuizApp.Services;

namespace SimpleQuizApp.ViewModels;

public partial class QuizViewModel : ViewModelBase
{
    [ObservableProperty] private Quiz _quiz;
    [ObservableProperty] private bool _hasQuiz;
    [ObservableProperty] private Bitmap? _imageSrc;
    [ObservableProperty] private bool _hasImage;

    public QuizViewModel(Quiz q, MainWindowViewModel main) : base(main)
    {
        Quiz = q;
        HasQuiz = q != null;
        _ = LoadImageAsync(q.ImageName);
    }

    private async Task LoadQuiz(Guid id)
    {
        var quiz = await QuizService.LoadQuizAsync(id);

        if (quiz == null) return;

        HasQuiz = true;
        Quiz = quiz;

        _ = LoadImageAsync(quiz.ImageName);
    }
    
    private async Task LoadImageAsync(string imageName)
    {
        ImageSrc = await ImageService.LoadAsync(imageName);
        HasImage = ImageSrc != null;
    }
    
    [RelayCommand]
    public void StartQuiz()
    {
        Main.NavigateTo(new PlayQuizViewModel(Quiz, Main));
    }

    [RelayCommand]
    public void EditQuiz()
    {
        Main.NavigateTo(new EditQuizViewModel(Quiz, Main));
    }
}