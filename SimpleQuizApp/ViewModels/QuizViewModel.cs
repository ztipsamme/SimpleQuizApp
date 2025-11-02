using System;
using System.Threading.Tasks;
using Avalonia.Media.Imaging;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SimpleQuizApp.Models;
using SimpleQuizApp.Servises;

namespace SimpleQuizApp.ViewModels;

public partial class QuizViewModel : ViewModelBase
{
    [ObservableProperty] private bool _isLoading;
    [ObservableProperty] private bool _hasQuiz;
    [ObservableProperty] private string _title;
    [ObservableProperty] private string _description;
    [ObservableProperty] private string? _coverImageName;
    [ObservableProperty] private Bitmap _coverImageSrc;
    [ObservableProperty] private bool _hasImage;
    private Quiz _quiz;

    public QuizViewModel(Guid id, MainWindowViewModel main) : base(main)
    {
        _ = LoadQuiz(id);
    }

    private async Task LoadQuiz(Guid id) {
        var quiz = await FileService.GetQuiz(id);
        
        if (quiz == null) return;
        
        HasQuiz = true;
        _quiz = quiz;
        Title = quiz.Title;
        Description = quiz.Description;
        CoverImageName = quiz.CoverImageName;
        
        _ = LoadImageAsync(CoverImageName);
    }
    
    private async Task LoadImageAsync(string imgName)
    {
        var (src, hasImage) = await FileService.GetImageAsync(imgName);
        CoverImageSrc = src;
        HasImage = hasImage;
    }

    [RelayCommand]
    public void StartQuiz()
    {
        Main.NavigateTo(new PlayQuizViewModel(_quiz, Main));
    }

    [RelayCommand]
    public void EditQuiz()
    {
        Main.NavigateTo(new EditQuizViewModel(_quiz, Main));
    }
}